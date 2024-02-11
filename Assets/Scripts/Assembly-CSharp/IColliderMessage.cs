using UnityEngine;

public interface IColliderMessage
{
	void IColliderSendMessage(GameObject colliderobj, ColliderMessage message);
}
