using System;
using GameCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace OfferPopup
{
	public class DealDialogHandler : BaseMonoBehaviour
	{
        [Header("Controllers")]
        [SerializeField]
        [FormerlySerializedAs("singleHeroOfferController")]
        private SingleHeroOfferPopupController singleHeroOfferPopupController;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("readDataOfferBundle")]
        private OfferBundleLoader offerBundleLoader;

        public SingleHeroOfferPopupController SingleHeroOfferPopupController
        {
            get
            {
                return singleHeroOfferPopupController;
            }
            set
            {
                singleHeroOfferPopupController = value;
            }
        }

        public OfferBundleLoader OfferBundleLoader
        {
            get
            {
                return offerBundleLoader;
            }
            set
            {
                offerBundleLoader = value;
            }
        }

        public void InitSingleHeroOffer(int heroID, DealKind type)
        {
            SingleHeroOfferPopupController.Init(heroID, type);
        }


	}
}
