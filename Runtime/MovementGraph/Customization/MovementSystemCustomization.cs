using System;
using JescoDev.MovementGraph.DefaultTargets;
using UnityEngine;

namespace JescoDev.MovementGraph {
    public abstract class MovementSystemCustomization : MonoBehaviour {

        public abstract void MoveTo(Vector3 worldPos);

        public abstract void MoveBy(Vector3 worldPos);
    }
}