var otherCollider : Collider;

function Update () {
if (otherCollider)
Physics.IgnoreCollision(this.collider, otherCollider);
}