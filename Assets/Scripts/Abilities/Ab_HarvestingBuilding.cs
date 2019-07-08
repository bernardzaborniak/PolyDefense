﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_HarvestingBuilding : Ability, IWorkerAssigneable<int>
{
    public RessourceType type;

    List<Ab_Worker> assignedWorkers = new List<Ab_Worker>();

    int currentWorkerNumber = 0;
    public int workerCapacity;

    public float width; //TODO change this into some kind of interface

    public void DepotRessource(int amount)
    {
        PlayerManager.Instance.AddRessource(type, amount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetWorkerNumber(2);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log(PlayerManager.Instance.GetIdleWorkersNumber());
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetWorkerNumber(1);
        }
    }

    //make aseperate abilty out of this worker assigners? - returns the noumber capped - if there are not so many idle workers aviable
    public int SetWorkerNumber(int newNumber)
    {


        //add more workers
        if (newNumber > currentWorkerNumber)
        {
            //first cap the number, 
            if (PlayerManager.Instance.GetIdleWorkersNumber() < newNumber-currentWorkerNumber)
            {
                newNumber = currentWorkerNumber + PlayerManager.Instance.GetIdleWorkersNumber();
            }

            for (int i = currentWorkerNumber; i < newNumber; i++)
            {
                Ab_Worker newWorker = PlayerManager.Instance.GetNearestIdleWorker(transform.position);
                newWorker.AssignToHarvesting(this);
                assignedWorkers.Add(newWorker);
            }
        }
        else
        {//release them from work
            for(int i = currentWorkerNumber-1; i >= newNumber; i--)
            {
                assignedWorkers[i].AssignToIdle();
                assignedWorkers.RemoveAt(i);
            }

        }

        currentWorkerNumber = newNumber;

        return newNumber;

    }
}
