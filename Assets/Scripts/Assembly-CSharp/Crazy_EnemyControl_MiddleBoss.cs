using UnityEngine;

public class Crazy_EnemyControl_MiddleBoss : Crazy_EnemyControl
{
	protected float bloodSlotHeight = 2.5f;

	private int line_count;

	protected override void preStart()
	{
		hurteffectposy = 2f;
		recycletime = 3f;
		base.preStart();
	}

	protected void InitBloodSlotBk(float size, GameObject slot)
	{
		GameObject gameObject = new GameObject("bloodslotbk", typeof(MeshFilter), typeof(MeshRenderer));
		gameObject.GetComponent<Renderer>().material = Resources.Load("Textures/boss_blood_slot_M") as Material;
		MeshFilter meshFilter = gameObject.GetComponent("MeshFilter") as MeshFilter;
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0.024f * size, 0.027f * size, -0.01f);
		array[1] = new Vector3(-0.63f * size, 0.027f * size, -0.01f);
		array[2] = new Vector3(0.024f * size, -0.069f * size, -0.01f);
		array[3] = new Vector3(-0.63f * size, -0.069f * size, -0.01f);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(0f, 0f);
		array2[3] = new Vector2(1f, 0f);
		array3[0] = 0;
		array3[1] = 2;
		array3[2] = 1;
		array3[3] = 1;
		array3[4] = 2;
		array3[5] = 3;
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.uv = array2;
		meshFilter.mesh.triangles = array3;
		gameObject.transform.parent = slot.transform.parent;
		gameObject.transform.localPosition = new Vector3(1.515f, 0f, 0f);
	}

	protected override void InitBloodSlot(float size)
	{
		GameObject gameObject = new GameObject("bloodslotparent");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = new Vector3(0f, bloodSlotHeight, 0f);
		bloodSlotObject = new GameObject("bloodslot", typeof(MeshFilter), typeof(MeshRenderer));
		bloodSlotObject.GetComponent<Renderer>().material = Resources.Load("Textures/boss_blood_M") as Material;
		MeshFilter meshFilter = bloodSlotObject.GetComponent("MeshFilter") as MeshFilter;
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		int[] array3 = new int[6];
		array[0] = new Vector3(0f, 0f, 0f);
		array[1] = new Vector3(-0.606f * size, 0f, 0f);
		array[2] = new Vector3(0f, -0.042f * size, 0f);
		array[3] = new Vector3(-0.606f * size, -0.042f * size, 0f);
		array2[0] = new Vector2(0f, 1f);
		array2[1] = new Vector2(1f, 1f);
		array2[2] = new Vector2(0f, 0f);
		array2[3] = new Vector2(1f, 0f);
		array3[0] = 0;
		array3[1] = 2;
		array3[2] = 1;
		array3[3] = 1;
		array3[4] = 2;
		array3[5] = 3;
		meshFilter.mesh.vertices = array;
		meshFilter.mesh.uv = array2;
		meshFilter.mesh.triangles = array3;
		bloodSlotObject.transform.parent = gameObject.transform;
		bloodSlotObject.transform.localPosition = new Vector3(1.515f, 0f, 0f);
		InitBloodSlotBk(size, bloodSlotObject);
	}

	protected override void updateBloodSlot()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("MainCamera");
		Vector3 vector = gameObject.transform.position - bloodSlotObject.transform.parent.position;
		vector.y = 0f;
		bloodSlotObject.transform.parent.forward = new Vector3(0f, 0f, 1f);
		bloodSlotObject.transform.localScale = new Vector3(GetHPPercent(), 1f, 1f);
	}

	protected override void Die()
	{
		dyingtime = 0f;
		Crazy_PlayerControl crazy_PlayerControl = target.GetComponent("Crazy_PlayerControl") as Crazy_PlayerControl;
		crazy_PlayerControl.AddGold(m_gold);
		crazy_PlayerControl.AddExp(m_exp);
		Crazy_GlobalData.cur_kill_number++;
		Crazy_Statistics.AddMonsterKillNumber(Crazy_MonsterType.MiddleBoss, m_id);
	}

	protected override bool IsPreAttack()
	{
		return preattacklasttime < preattacktime;
	}

	protected override void OnAttack()
	{
		base.OnAttack();
		Vector2 original = new Vector2(base.transform.position.x, base.transform.position.z);
		Vector2 forward = new Vector2(base.transform.forward.x, base.transform.forward.z);
		ResetLine();
		Vector2 original2 = Crazy_Global.RotatebyAngle(original, forward, sp.angle, sp.length);
		Vector3 vector = new Vector3(original2.x, 0f, original2.y);
		DrawLine(base.transform.position, vector, new Color(1f, 0f, 0f, 0.2f));
		Vector2 forward2 = Crazy_Global.Rotate(forward, sp.fixangle);
		for (float num = 0f; num <= sp.rangeangle; num += 1f)
		{
			Vector2 vector2 = Crazy_Global.RotatebyAngle(original2, forward2, num, sp.rangelength);
			Vector3 end = new Vector3(vector2.x, 0f, vector2.y);
			Vector2 vector3 = Crazy_Global.RotatebyAngle(original2, forward2, 0f - num, sp.rangelength);
			Vector3 end2 = new Vector3(vector3.x, 0f, vector3.y);
			DrawLine(vector, end, new Color(1f, 0f, 0f, 0.2f));
			DrawLine(vector, end2, new Color(1f, 0f, 0f, 0.2f));
		}
	}

	public override void SetHint(bool hint)
	{
		base.SetHint(hint);
		ResetLine();
	}

	public void ResetLine()
	{
		LineRenderer lineRenderer = base.gameObject.GetComponent("LineRenderer") as LineRenderer;
		if (lineRenderer != null)
		{
			line_count = 0;
			lineRenderer.SetVertexCount(line_count);
		}
	}

	public void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		if (usehint)
		{
			LineRenderer lineRenderer = base.gameObject.GetComponent("LineRenderer") as LineRenderer;
			if (lineRenderer == null)
			{
				lineRenderer = base.gameObject.AddComponent<LineRenderer>() as LineRenderer;
				lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
			}
			lineRenderer.SetWidth(0.1f, 0.1f);
			lineRenderer.SetColors(color, color);
			line_count += 2;
			lineRenderer.SetVertexCount(line_count);
			lineRenderer.SetPosition(line_count - 2, start);
			lineRenderer.SetPosition(line_count - 1, end);
		}
	}
}
