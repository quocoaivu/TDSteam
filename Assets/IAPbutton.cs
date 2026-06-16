using UnityEngine;

// TODO: re-wire after IAP migration. See Docs/OPENIAB_REMOVAL_REINSTALL.md
// OpenIAB removed 2026-05-19. Original product IDs preserved below:
//   buy1 -> com.developer.kingdom.defense.iap1
//   buy2 -> com.developer.kingdom.defense.iap2
//   buy3 -> com.developer.kingdom.defense.iap3
//   buy4 -> com.developer.kingdom.defense.iap4
//   buy5 -> com.developer.kingdom.defense.iap5
public class IAPbutton : MonoBehaviour
{
    public void buy1() { Debug.Log("[IAP-stub] buy1 (iap1)"); }
    public void buy2() { Debug.Log("[IAP-stub] buy2 (iap2)"); }
    public void buy3() { Debug.Log("[IAP-stub] buy3 (iap3)"); }
    public void buy4() { Debug.Log("[IAP-stub] buy4 (iap4)"); }
    public void buy5() { Debug.Log("[IAP-stub] buy5 (iap5)"); }
}
