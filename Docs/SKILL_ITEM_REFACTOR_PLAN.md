# Kế hoạch Refactor — Tower Roguelite: Item mở khóa Skill (Hệ B)

> Tài liệu chốt thiết kế + kế hoạch refactor. Ngày: 2026-06-27.
> Phạm vi: chuyển hệ Tower sang mô hình roguelite — bỏ level, item mở khóa skill, skill tree khuếch đại.
> Làm **Archer (tower_id 0) vertical slice trước**, test xong mới nhân ra 5 tower.

---

## 1. Bối cảnh & mô hình đã khóa

### 1.1 Triết lý
| Trục | Nguồn dữ liệu | Vai trò |
|------|---------------|---------|
| **Base weapon** | `tower_parameter.txt` (cố định, **bỏ level**) | Sàn — luôn có, đảm bảo tower không bao giờ "trống" |
| **Skill (4/tower)** | **skill-item → Ability** (Hệ B) | **Item = CÓ/KHÔNG** (bật/tắt cơ chế) |
| **Stat / on-hit augment** | item-stat → BuffHolder (Hệ A cũ) | Cộng số, stackable |
| **Khuếch đại** | Skill tree (meta, vĩnh viễn) | **MẠNH/YẾU** |
| **Tier skill** | = `rarity` của skill-item (0/1/2) | Mức sàn của skill |

### 1.2 Nguyên tắc cốt lõi (user xác nhận)
- **Item quyết định CÓ/KHÔNG. Skill tree quyết định MẠNH/YẾU.** Hai trục độc lập.
- **Chỉ 1 cổng bật skill = item.** Không gate chéo: có item nhưng skill tree chưa nâng → skill **vẫn chạy ở mức sàn**, không bị tắt.
- Tower **không có level** như một trục tiến triển — level co lại thành 1 base cố định.
- Skill tree **vĩnh viễn (meta)**: sang map khác base weapon mạnh lên do tree đã nâng → mọi tower cùng loại ngang cấp.
- Để "ván xui không có item ≠ thua chắc": **base weapon luôn có** (sàn). (Có thể bổ sung trục in-run sau nếu cần.)

### 1.3 Hai loại item — KHÔNG trùng effect
| Loại | Cơ chế | Slot | Ví dụ Archer |
|------|--------|------|--------------|
| **Skill-item (Hệ B)** | mở 1 trong 4 Ability | skill slot (chọn 2/4) | Multi Arrow, Frost Arrow→Freezing, Shadow Quiver, Marksman Lens |
| **Augment-item (Hệ A)** | cộng số / on-hit thụ động *thuần* | stat slot (stackable) | Keen Arrowhead (+dmg), Venom Tip (poison), Assassin's Mark (critdmg) |

**Dọn trùng duy nhất:** Frost Arrow (Slow, id=20) trùng với skill Freezing Arrow → chuyển Frost Arrow thành **skill-item của Freezing**, bỏ vai stat-Slow.
**Không trùng:** Assassin's Mark (CritDamage) ≠ skill Assassinate (giết tức thì) → giữ làm augment-item.

---

## 2. 4 Skill của Archer & map sang skill-item

| branch | skill_id | Skill (đã có Ability) | Skill-item | rarity (tier) |
|--------|----------|------------------------|------------|---------------|
| 0 | 0 | Multi-Shot | **Multi Arrow** (mới) | 0 |
| 0 | 1 | Freezing Arrow | **Frost Arrow** (đổi id=20) | 1 |
| 1 | 0 | Assassinate | **Shadow Quiver** (mới) | 2 |
| 1 | 1 | Critical Shot | **Marksman Lens** (mới) | 1 |

> Mô tả in-game (file `tower_skill_description_lg_vi.txt`):
> - Multi-Shot: "Bắn một trận mưa tên xuống đầu kẻ thù gây sát thương diện rộng."
> - Freezing Arrow: "Bắn mũi tên gây đóng băng kẻ thù."
> - Assassinate: "Có xác suất {0}% tiêu diệt ngay lập tức 1 kẻ thù."
> - Critical Shot: "Kẻ địch nhận thêm {0}% sát thương từ nguồn khác trong thời gian dính kỹ năng."

---

## 3. Slot layout đề xuất (Archer)

```
ARCHER (tower_id 0, KHÔNG level)
│  Base weapon (innate, tower_parameter.txt dòng 0):
│     dmg 3 | atk_spd 1.2 | range 1.75 | proj_spd 5 | crit 5%/x2.0 | First | air ✓
│     → mặc định bắn 1 mũi tên thường.
│
├── 4 ô TowerEquipment (SLOT_COUNT=4, generic), trong đó:
│     • tối đa 2 skill-item (chọn 2/4 skill) — chặn bằng check GetEquipBlock
│     • còn lại cho augment-item (stat / on-hit)
```

> Không tách cứng "skill slot" vs "stat slot" — giữ 4 ô chung, chỉ giới hạn **≤2 skill-item** để đỡ phải đập lại UI.

---

## 4. Hiện trạng tái dùng được (KHÔNG làm lại)

- **`TowerEquipment.cs`** — đã có hệ trang bị 4 ô (`SLOT_COUNT=4`), `Equip/Unequip` + áp stat qua `BuffHolder` (recompute-from-source). Hệ A chạy đủ.
- **`TurretMasteryHandler.EquipItem(slot, level)` → `UnlockUltimate(level)`** — đã có đường bật skill, nhưng **chưa ai gọi**.
- **16 Ability class** + param (`tower_skill_parameter.txt`, `TurretAbilitySpec`) — đã có, gate bằng cờ `unlock`.
- Comment `TurretAbilitySpec.cs` *"an ability (= an item)"* → ý đồ "skill = item" đã có sẵn.
- Sprite icon skill format `ultimate_{id}_{branch}_{skillID}` — tái dùng được cho skill-item.

---

## 5. Gap phải lấp

1. `ItemSpec`/`TowerItem` **không có tham chiếu skill** (branch/skill_id).
2. `TowerEquipment` **không nối** sang mastery handler (equip item ≠ bật skill).
3. Ability **không có hàm tắt** (chỉ có bật) — nhưng dễ: set `unlock=false`.
4. Branch vẫn suy từ **level** (`GetUltimateBranchByLevel`).
5. Resolve (branch, skill_id) → Ability instance: hiện index theo slot, cần lookup.

---

## 6. Các bước refactor (theo thứ tự)

### Phase 1 — Data schema
**File:** `Assets/Resources/parameters/items/tower_item_parameter.txt`
- Thêm 2 cột: `skill_branch, skill_id` (rỗng = item-stat thường).
- Thêm 3 skill-item mới + sửa Frost Arrow cho Archer:
```
30,0,Multi Arrow,    ,,,,,, 0,0, 0, .../item_multi
20,0,Frost Arrow,    ,,,,,, 0,1, 1, .../item_frost   ← bỏ stat Slow, thành skill-item
31,0,Shadow Quiver,  ,,,,,, 1,0, 2, .../item_assa
32,0,Marksman Lens,  ,,,,,, 1,1, 1, .../item_marks
```
> `rarity` (cột có sẵn) = tier skill.

### Phase 2 — Parser + struct
**Files:** `ItemSpec.cs`, `TowerDataLoader.cs`, `ItemSpecCatalog.cs`, `TowerItem` (runtime, trong `ItemFactory`)
- `ItemSpec`: thêm `int skillBranch; int skillId;` (mặc định -1 = không phải skill-item).
- `TowerDataLoader`: parse 2 cột mới.
- `TowerItem`: mang `skillBranch/skillId`; thêm helper `bool IsSkillItem => skillId >= 0;`.

### Phase 3 — Bridge: equip item ⇄ bật skill ★ TRỌNG TÂM
**File:** `TowerEquipment.cs`
- `Equip(item)`: sau khi add, nếu `item.IsSkillItem` → gọi `tower.towerUltimateController` kích hoạt Ability (branch, skillId) ở tier = `item.rarity`.
- `Unequip(item)`: nếu skill-item → tắt Ability tương ứng.
- Giữ nguyên đường stat cũ cho augment-item.

**File:** `TurretMasteryHandler.cs`
- Đổi `EquipItem(slotIndex,…)` → resolve theo (branch, skillId): `ActivateSkill(int branch, int skillId, int tier)` và `DeactivateSkill(int branch, int skillId)`.
- Thêm `FindAbility(branch, skillId)` duyệt `listTowerUltimate` so khớp field branch/skillId.

**Files:** các Ability + `TurretMasteryShared.cs`
- Expose `branch`/`skillId` (private → property/field công khai để lookup).
- Thêm base `public virtual void LockUltimate() { unlock = false; }`.

### Phase 4 — Bỏ level / branch-theo-level
**File:** `TowerParameterManager.cs`
- Bỏ `GetUltimateBranchByLevel`, hằng `ULTIMATE_LEVEL_BRANCH_0/1`, `MAX_BASE_LEVEL` (hoặc giữ `Level` như hằng để khỏi sửa rải rác — xem Rủi ro #2).

**File:** `TurretAbilityEnhanceSwitchHandler.cs` (nút nâng-skill-bằng-vàng)
- **Bỏ/ngừng dùng** — vai trò mở+nâng skill chuyển hết sang item.

**File:** `TurretSynopsis.cs`
- 2 chỗ gọi `GetUltimateBranchByLevel` → đọc branch từ item đã trang bị (mô tả skill theo item, không theo level).

### Phase 5 — Skill tree khuếch đại skill (GHI NỢ)
**Vấn đề:** skill dmg (vd Multi-Shot `damagePerArrow`) lấy từ **param skill riêng**, KHÔNG từ tower dmg → skill tree dmg hiện **không** tự khuếch đại skill.
**2 lựa chọn (làm sau vertical slice):**
- (a) Thêm node skill tree chuyên biệt ("+1 mũi", "+x% dmg skill").
- (b) Cho skill dmg scale theo tower dmg (sửa công thức trong Ability).
- Vertical slice: tạm để skill tree chỉ khuếch đại **base weapon**; skill chạy theo tier item.

### Phase 6 — Dọn trùng lặp
- Frost Arrow: bỏ khỏi đường `StatType.Slow` (đã thành skill Freezing). Kiểm `BuffKeysToTurret.SlowOnHitIncrementCommon` còn ai dùng; nếu không, để lại cho item Slow khác hoặc gỡ.
- Xác nhận Assassin's Mark (CritDamage) **giữ** làm augment-item.

### Phase 7 — Prefab / Inspector (cần Unity)
- Prefab Archer: `TurretMasteryHandler.listTowerUltimate` phải chứa **đủ 4 Ability** (hiện có thể chỉ 2). Mỗi Ability set đúng branch/skillId.
- ⚠️ **Không sửa prefab/scene khi Unity đang mở** — hướng dẫn qua Inspector, hoặc user đóng Unity để sửa file trực tiếp.

---

## 7. Rủi ro / điểm cần để mắt

1. **`listTowerUltimate` đủ 4 Ability?** Nếu prefab chỉ wire 2 (1 branch), phải thêm 2 component + clip FX → việc thủ công trong Unity.
2. **Đừng xóa sạch `Level` vội** — nhiều nơi đọc `towerModel.Level` (synopsis, sprite icon `ultimate_{id}_{branch}_{skillID}`). An toàn: giữ field như hằng, chỉ bỏ *logic thay đổi* nó.
3. **Sprite icon skill** dùng format `ultimate_{id}_{branch}_{skillID}` — map đẹp với skill-item, tái dùng được.
4. **SLOT_COUNT=4 generic** vs ý "skill slot + stat slot": không tách cứng — cho equip ≤4 item bất kỳ, giới hạn **≤2 skill-item** bằng check trong `GetEquipBlock`.

---

## 8. Test — Archer vertical slice

1. Build Archer → bắn 1 mũi (base, chưa item). ✓
2. Trang bị Multi Arrow (rarity 0) → bắn nhiều mũi mức sàn. ✓
3. Tháo Multi Arrow → quay lại 1 mũi (`unlock=false`). ✓
4. Trang bị Multi Arrow rarity cao → nhiều mũi hơn. ✓
5. Trang bị skill-item thứ 3 → bị chặn (giới hạn 2). ✓
6. Frost Arrow → đóng băng (skill), không còn double slow. ✓

---

## 9. Thứ tự thực thi đề xuất

**Phase 1 → 2 → 3 → 4 (thuần code) → 7 (Unity, cùng user) → test → 5, 6 (polish).**
Phase 3 là tim của refactor.

---

## Phụ lục — file dữ liệu liên quan
- Base stat tower: `Assets/Resources/parameters/tower_parameter.txt`
- Skill param: `Assets/Resources/parameters/towerskills/tower_skill_parameter.txt`
- Skill tree: `Assets/Resources/parameters/tower_skilltree_parameter.txt`
- Item: `Assets/Resources/parameters/items/tower_item_parameter.txt`
- Mô tả skill: `Assets/Resources/parameters/description/tower_skill_description_lg_*.txt`
- Cân bằng base+tree: `Docs/balance_tower_base_skilltree.txt`
