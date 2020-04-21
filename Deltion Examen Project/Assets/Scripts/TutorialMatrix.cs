using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMatrix : MonoBehaviour
{
   public enum MatrixType
    {
        Damage,
        Healing,

    }

    public MatrixType myType;
    private Player currentPlayer;
    private bool waiting;

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if(!currentPlayer)
                currentPlayer = other.GetComponent<Player>();
        }

        if (currentPlayer)
        {
            switch (myType)
            {
                case MatrixType.Damage:
                    DealDamage();
                    break;
                case MatrixType.Healing:
                    Heal();
                    break;
            }
        }
    }

    private void DealDamage()
    {
        if(currentPlayer.GetHp() > currentPlayer.maxHp / 2)
        {
            currentPlayer.TakeDamage(10, null);
            TutorialManager.instance.PlayerDamaged();
        }
    }

    private void Heal()
    {
        if (currentPlayer.GetHp() < currentPlayer.maxHp)
        {
            currentPlayer.Heal(10, 0);
            TutorialManager.instance.PlayerHealed();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if(currentPlayer == other.GetComponent<Player>())
            {
                currentPlayer = null;
            }
        }
    }
}
