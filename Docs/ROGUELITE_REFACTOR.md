# Tower Defense → Roguelite — Tổng hợp Refactor

> Cập nhật: 2026-06-16. Tài liệu này tóm tắt việc chuyển game TD (cảm hứng Kingdom Rush)
> sang **Tower Defense Roguelite**, tập trung refactor hệ **Tower**.

---

## 1. Mục tiêu & định hướng

- Chuyển Tower sang mô hình modular, dễ mở rộng + hỗ trợ combo/synergy.
- **2 hệ tiền tệ tách biệt:**
  - **Skill Point** = tiền META (giữa các trận), mua node skill tree **vĩnh viễn**.
  - **Vàng in-match** (`GameRecord.Money`, không persist) = xây tower + reroll item trong trận.
- Skill **KHÔNG random mỗi run** — vĩnh viễn, mua qua skill tree.
- Item là **in-run** (nhặt trong trận, reset mỗi run).

---

## 2. Hai hệ chính

### A. Skill Tree (META — ngoài trận)
- Cây node STAT thuần (dmg/range/reload/crit/pierce…) **theo loại Tower**, mua bằng Skill Point.
- Stat runtime tower = **base (level 0) + Σ node đã mở**. Tower không còn lên cấp in-run.
- Mở từ **Worldmap** (panel `PanelArcherSkillTree`).

### B. Item (IN-RUN — trong trận)
- Tái dùng nguyên **22 ability mastery cũ** (`TurretXMasteryYAbilityZ`) — nguồn kích hoạt đổi từ
  "mua mastery" sang "trang bị Item" (gọi `UnlockUltimate`).
- **Per-tower**: mỗi item chỉ hợp 1 loại tower (vì ability là component vật lý trên prefab canonical).
  Mỗi tower equip được **2 item** = 2 ability trên prefab canonical của nó.
- Nguồn item: **drop từ enemy** (auto-collect) + **reroll shop bằng vàng**.

---

## 3. Quyết định thiết kế cốt lõi

| Vấn đề | Quyết định |
|---|---|
| Skill data | Giữ trên file CSV (`Resources/parameters/...`), không chuyển ScriptableObject |
| Prefab canonical | Tier cao nhất mỗi tower: Archer/Knight/Cannon/Dragon = L4, Priest = L3 |
| Branch ability theo level | L4 → branch 1, L3 → branch 0 (`GetUltimateBranchByLevel`) |
| GlobalUpgrade cũ | Giữ riêng, KHÔNG gộp — skill tree là hệ MỚI song song |
| Item model | **Per-tower** (tái dùng ability, không sửa prefab) |
| Nguồn SkillPoint | Thắng campaign theo **số sao** (1 SP/sao) |
| Nguồn item | Drop từ enemy (8%/kill) + reroll shop (vàng) |
| Pickup UX | **Auto-collect + visual nổi** (đúng convention gem/gold; game không có click-nhặt) |

---

## 4. Tiến độ theo Phase

- ✅ **Phase 0.5** — `TowerPool` luôn spawn prefab canonical (helper `CanonicalLevel`).
- ✅ **Phase 1** — 2 store mới: `TowerSkillPointStore` (tiền SP), `TowerSkillTreeStore` (node đã mở).
- ✅ **Phase 2** — CSV `tower_skilltree_parameter.txt` + `TowerSkillNode`/`TowerSkillTreeSpec` + loader.
- ✅ **Phase 3** — `TurretEntity` đọc stat = base + cây (`GetStatWithTree`); `Upgrade()` thành no-op.
- ✅ **Phase 4** — ẩn toàn bộ nút upgrade/mastery cũ trong popup tower (giữ Sell/range/info).
- ✅ **Phase 5** — UI Skill Tree: `TowerSkillTreePanel` + `SkillTreeNodeButton`, wire vào Worldmap.
- ✅ **Phase 6** — Item equip: `TurretMasteryHandler.EquipItem`, UI `TowerItemPanel` trong popup tower.
- ✅ **Phase 7** — Kinh tế in-run:
  - 7a: SkillPoint khi thắng + reset inventory mỗi run + drop từ enemy.
  - 7b-1: chỉ báo item drop nổi (`ItemDropHandler` qua `FXPool`).
  - 7b-2: reroll shop bằng vàng (`ItemShopPanel`).
- ⏳ **Cloud-sync 2 store** — chưa làm (xem mục 9).

---

## 5. File chính

### Data / Store (`Assets/Scripts/Data/`)
- `TowerSkillPointStore.cs` — tiền META, file `/towerSkillPointInfor.dat`. API: `GetCurrentSkillPoint`/`AddSkillPoint`/`TrySpend`.
- `TowerSkillTreeStore.cs` — node đã mở per-tower, file `/towerSkillTreeInfor.dat`. API: `IsNodeUnlocked`/`GetUnlockedNodes`/`UnlockNode`.

### Parameter (`Assets/Scripts/Parameter/`)
- `TowerSkillNode.cs` (struct), `TowerSkillTreeSpec.cs` (lookup cache).
- `TurretAbilitySpec.GetSkillName(...)` — tên item từ CSV.
- `TowerParameterManager.GetStatWithTree(id)` — stat = base + cây.

### Skill Tree UI (`Assets/Scripts/Upgrade/`)
- `TowerSkillTreePanel.cs` (extends GameplayDialogHandler), `SkillTreeNodeButton.cs`, `SkillTreeNodeState.cs`.

### Item (`Assets/Scripts/Items/`)
- `TowerItem.cs` — model item runtime (towerID, slotIndex, level, name).
- `ItemInventory.cs` — bag in-run (lazy singleton, không persist).
- `ItemFactory.cs` — tạo item ngẫu nhiên (dùng chung drop + shop).
- `ItemDropRoller.cs` — roll drop khi enemy chết.
- `TowerItemPanel.cs` + `ItemSlotButton.cs` — UI equip trong popup tower.
- `ItemShopPanel.cs` + `ItemShopOfferButton.cs` — reroll shop in-match.

### Gameplay (sửa)
- `Gameplay/Tower/Ultimate/TurretMasteryHandler.cs` — `EquipItem`/`GetEquippedLevel`.
- `Gameplay/Tower/EnhanceTurretDialogHandler.cs` — field `itemPanel` + gọi `Init`.
- `Gameplay/Economy/ItemDropHandler.cs` — visual item drop (mirror CrystalHandler).
- `Gameplay/Effects/FXPool.cs` — pool item drop (`GetDroppedItem`).
- `Gameplay/Enemy/EnemyData.cs` — hook `ItemDropRoller.TryDropOnKill` trong `Dead()`.
- `Gameplay/Core/GameplayDirector.cs` — `ItemInventory.Clear()` mỗi match start.
- `Gameplay/Core/GameRuleHandler.cs` — `AwardSkillPoint()` khi thắng.

### Editor builders (`Assets/Scripts/Editor/`)
- `SkillTreeBuilder.cs` — menu **Tools > Tower Skill Tree > Build Archer Nodes**.
- `ItemSlotBuilder.cs` — menu **Tools > Tower Item > Build Item Slots**.
- `ItemShopBuilder.cs` — menu **Tools > Tower Item > Build Item Shop**.

### Data files (`Assets/Resources/parameters/`)
- `tower_skilltree_parameter.txt` — node cây skill (hiện 14 node Archer id 0).
- `TowerSkills/tower_skill_parameter.txt` — param ability = data Item (5 tower × 2 branch × 2 skill × 3 level).

---

## 6. Tower / ability map

| Tower ID | Tên | Canonical | Branch | 2 ability equip được (slot 0 / slot 1) |
|---|---|---|---|---|
| 0 | Archer | L4 | 1 | Assassinate / Critical Shot |
| 1 | Knights/Infantry | L4 | 1 | Dexterity / Blade Storm |
| 2 | Canonneer | L4 | 1 | Heavy Boulder / ThunderBolt |
| 3 | Magic Dragon | L4 | 1 | Shove / Intimidate |
| 4 | Priest | L3 | 0 | (branch 0) |

> Lưu ý: ability nhánh 0 của tower L4 (Multi-Shot, Freezing…) KHÔNG có trên prefab canonical → không equip được (per-tower theo prefab thật).

---

## 7. Wire trong Unity Editor

### Skill Tree (Worldmap)
1. Prefab `PanelArcherSkillTree` → chạy **Build Archer Nodes** trên panel gốc.
2. `UIRootHandler` (Worldmap): gán `archerSkillTreePrefab`.
3. Nút mở: component `ArcherSkillTreeSwitchHandler` (towerID=0) → OnClick.

### Item equip (popup tower — scene Gameplay)
- Object **`Upgrade Tower`** (dưới `Canvas 1 - Fixed > Control Tower Group`).
- Tạo child `TowerItemPanel` → chạy **Build Item Slots** → gán field `Item Panel` trên `EnhanceTurretDialogHandler`.

### Item drop visual (tùy chọn)
- Duplicate prefab `GoldDropped` → đổi tên **`ItemDropped`** → swap script `ItemDropHandler` → gán vào `FXPool.itemDropControllerPrefab`.

### Reroll shop (scene Gameplay)
- Tạo `ItemShopPanel` (con của `Canvas 1 - Fixed`, **Scale = 1,1,1**, inactive) → chạy **Build Item Shop**.
- Nút mở (trên HUD): OnClick → `ItemShopPanel.OpenShop()`.
- Nút đóng (builder tự tạo) + reroll (builder tự tạo).

---

## 8. Test nhanh

1. Thắng 1 trận campaign → vào Worldmap, SkillPoint tăng theo sao → mua node trong skill tree.
2. Trong trận: giết quái → item rớt (auto vào inventory, có chữ nổi nếu đã dựng `ItemDropped`).
3. Mở popup tower → equip item → ability bật.
4. Mở reroll shop → mua item bằng vàng / reroll đổi offer.

---

## 9. Việc còn lại

### Cloud-sync 2 store (TowerSkillPoint / TowerSkillTree) — CHƯA làm
Kiến trúc (mẫu = `GlobalUpgradeStore`):
- DTO `[Serializable]` ở `Assets/Scripts/CloudSave/`: `PlayerRecord_X` (List) + `_Unique`.
- Restore: `RecordRestoreDeliver.DispatchToAllDataWriter` → `Store.RestoreDataFromCloud(userData_X)`.
- Backup: `RecordCloudSaverAndroid.cs` gom local → DTO → upload (**chưa đọc xong**).

Các bước:
1. Tạo `PlayerRecord_TowerSkillPoint` + `PlayerRecord_TowerSkillTree`(+`_Unique`).
2. Thêm `RestoreDataFromCloud(...)` vào 2 store.
3. Thêm field + dòng gọi trong `RecordRestoreDeliver`.
4. Populate 2 DTO trong `RecordCloudSaverAndroid` (backup).

⚠️ Saver tên `*Android` → cloud có thể **chỉ chạy trên Android**, Steam/PC có thể vô tác dụng — xác nhận trước.

### Tuning (hằng số, dễ chỉnh)
- Drop chance: `ItemDropRoller.DROP_CHANCE_PERCENT = 8`.
- SkillPoint/sao: `GameRuleHandler.AwardSkillPoint` (hiện = số sao).
- Shop: `ItemShopPanel.buyCost = 100`, `rerollCost = 50` (SerializeField, tune Inspector).

### Khác
- Cây skill tree mới chỉ có Archer (id 0) — cần thiết kế node cho 4 tower còn lại.
- Item drop hiện đồng đều random — cân nhắc drop table theo độ hiếm/level.

---

## 10. Lưu ý kỹ thuật

- **Animator**: dùng làm clip storage, `animator.Play(name)` trực tiếp (xem CLAUDE.md).
- **Pooling**: drop/visual qua `Common.GameObjectPool` + `FXPool`, KHÔNG Instantiate/Destroy trong loop.
- **TMP** mọi nơi (không legacy Text).
- 2 store mới là **lazy singleton plain class** (khác GlobalUpgradeStore là MonoBehaviour) — không cần wire scene.
- Item in-run reset mỗi match qua `GameplayDirector.SetDefaultGameData`; equipped reset qua tower pool.
- UI popup gameplay nằm dưới `Canvas 1 - Fixed` (Screen Space - Camera), child **Scale = 1,1,1**.
