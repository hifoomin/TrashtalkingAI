using RoR2;
using UnityEngine;

namespace TrashtalkingAI
{
    public class TrashtalkingController : MonoBehaviour
    {
        public float timer = 0f;
        public bool canTrashTalk = true;
        public float interval = 1f;
        public int randomAssignedID;

        public float globalTimer = 0f;
        public bool canTrashTalkGlobally = true;
        public float globalInterval = 1f;

        public void Start()
        {
            globalInterval = Main.globalTrashTalkingInterval.Value;
            interval = Main.trashTalkingInterval.Value;
            randomAssignedID = Run.instance.runRNG.RangeInt(0, 300);
        }

        public void FixedUpdate()
        {
            if (canTrashTalk == false)
            {
                timer += Time.fixedDeltaTime;
                if (timer >= interval)
                {
                    canTrashTalk = true;
                    timer = 0f;
                }
            }
            if (canTrashTalkGlobally == false)
            {
                globalTimer += Time.fixedDeltaTime;
                if (globalTimer >= globalInterval)
                {
                    canTrashTalkGlobally = true;
                    globalTimer = 0f;
                }
            }
        }
    }
}