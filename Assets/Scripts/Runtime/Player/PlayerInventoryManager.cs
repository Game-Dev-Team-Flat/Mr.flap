using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField]
    private Vector3 itemPosition;
    private PlayerInfo playerInfo;
    private GameObject m_itemHand;
    public GameObject itemHand => m_itemHand;

    private void Awake()
    {
        playerInfo = GetComponent<PlayerInfo>();
        m_itemHand = Instantiate(playerInfo.inventory[playerInfo.inventorySlotNumber].item, transform.position + playerInfo.eyesOfObject.forward * itemPosition.z + playerInfo.eyesOfObject.up * itemPosition.y + playerInfo.eyesOfObject.right * itemPosition.x, playerInfo.eyesOfObject.rotation, playerInfo.eyesOfObject);
        m_itemHand.layer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        SwapSlot();
    }

    private void SwapSlot()
    {
        int defaultSlotNumber = playerInfo.inventorySlotNumber;

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) // 휠로 슬롯 변경
        {
            playerInfo.inventorySlotNumber++;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            playerInfo.inventorySlotNumber--;
        }

        for (int i = 0; i < playerInfo.inventory.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)(49 + i)))   // alpha Number로 슬롯 변경
            {
                playerInfo.inventorySlotNumber = i;
            }
        }

        if (playerInfo.inventorySlotNumber != defaultSlotNumber) // 슬롯이 변경됐을 때
        {
            DestroyAndInstantiateItem(); // 기존 무기를 제거하고 Slot Number에 맞는 아이템 소환
        }
    }

    private void DestroyAndInstantiateItem()
    {
        Destroy(itemHand);
        m_itemHand = Instantiate(playerInfo.inventory[playerInfo.inventorySlotNumber].item, transform.position + playerInfo.eyesOfObject.forward * itemPosition.x + playerInfo.eyesOfObject.up * itemPosition.y + playerInfo.eyesOfObject.right * itemPosition.x, playerInfo.eyesOfObject.rotation, playerInfo.eyesOfObject);
        m_itemHand.layer = LayerMask.NameToLayer("Player");
    }
}