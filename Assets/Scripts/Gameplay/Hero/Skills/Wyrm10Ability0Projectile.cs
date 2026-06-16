using System;
using UnityEngine;

namespace Gameplay
{
	public class Wyrm10Ability0Projectile
	{
        public Vector3 startPos;

        public Vector3 endPos;

        public Vector3 midAnchor;

        public float duration;

        private float countdown;

        public GameObject projectile;


        private GameObject explodePrefab;
        public Wyrm10Ability0Projectile(GameObject projPrefab, GameObject explodePrefab, Vector3 startPos, Vector3 endPos, float detourDis, float duration)
		{
			this.startPos = startPos;
			this.endPos = endPos;
			this.explodePrefab = explodePrefab;
			Vector3 a = endPos - startPos;
			Vector3 vector = new Vector3(-a.y, a.x, 0f);
			Vector3 normalized = vector.normalized;
			midAnchor = startPos + a * UnityEngine.Random.Range(0.2f, 0.55f) + normalized * UnityEngine.Random.Range(-detourDis, detourDis);
			projectile = ObjectCache.Spawn(projPrefab, startPos);
			this.duration = duration;
			countdown = duration;
		}

		public bool OnUpdate(float dt)
		{
			countdown -= dt;
			float d = (duration - countdown) / duration;
			Vector3 position = startPos + (endPos - startPos) * d;
			if (countdown <= 0f)
			{
				MonoSingleton<LensHandler>.Instance.ShakeNormal();
				projectile.Recycle();
				ObjectCache.Spawn(explodePrefab, position);
				return false;
			}
			projectile.transform.position = position;
			return true;
		}
	}
}
