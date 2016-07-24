 using UnityEngine;
using System.Collections;

public class Pony : MonoBehaviour
{
    public float movesPerMinute = 5.0f;
    public float moveRadius = 10.0f;
    public float moveSpeed = 1.0f;
    public float averageLiveSpan = 60.0f;
    public float corpseRemovalTime = 10.0f;
    public bool removeCorpse = true;

    private Animation m_anim; 
    private Vector3 m_targetPos;
    private bool m_dead = false;
    private float m_lastAnimTime = 0.0f;

	void Start ()
    {
        m_anim = GetComponent<Animation>();
        m_anim.Play("Pony_Rig|Idle");
        m_targetPos = transform.position;

        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material.SetColor("_mask1", Random.ColorHSV(0.0f, 1.0f, 0.0f, 0.8f, 0.2f, 1.0f, 1.0f, 1.0f));
        renderer.material.SetColor("_mask2", Random.ColorHSV(0.0f, 1.0f, 0.0f, 0.8f, 0.2f, 1.0f, 1.0f, 1.0f));
    }
	
	void Update ()
    {
        if (Random.Range(0.0f, 60 / movesPerMinute) < Time.deltaTime)
        {
            Vector2 pos = Random.insideUnitCircle * moveRadius;
            m_targetPos = new Vector3(pos.x, 0, pos.y);
        }

        Vector3 displacement = m_targetPos - transform.position;
        
        if (Random.Range(0.0f, averageLiveSpan) < Time.deltaTime || m_dead)
        {
            m_dead = true;

            if (m_lastAnimTime > m_anim["Pony_Rig|Die"].time)
            {
                m_anim.Stop();
                if (removeCorpse)
                {
                    StartCoroutine(RemoveBody());
                }
            }
            else
            {
                m_anim.Play("Pony_Rig|Die");
                m_lastAnimTime = m_anim["Pony_Rig|Die"].time;
            }
        }
        else if (displacement.magnitude > 0.05f)
        {
            transform.rotation = Quaternion.LookRotation(displacement, Vector3.up);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            m_anim.Play("Pony_Rig|Walk");
        }
        else
        {
            m_anim.Play("Pony_Rig|Idle");
        }
	}

    IEnumerator RemoveBody()
    {
        yield return new WaitForSeconds(corpseRemovalTime);
        Destroy(gameObject);
    }
}
