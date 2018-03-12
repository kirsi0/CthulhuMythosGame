using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuickSorting
{


	static public void QuickSort (List<BasicBlock> list, int left, int right)
	{
		if (left > right) {
			return;
		}
		int storeIndex = Partition (list, left, right);
		QuickSort (list, left, storeIndex - 1);
		QuickSort (list, storeIndex + 1, right);
	}

	static private int Partition (List<BasicBlock> list, int left, int right)
	{
		int storeIndex = left;
		BasicBlock pivot = list [right];
		for (int i = left; i < right; i++) {
			if ((int)list [i].BlockType < (int)pivot.BlockType) {
				Swap (list, storeIndex, i);
				storeIndex++;
			}
		}
		Swap (list, right, storeIndex);
		return storeIndex;
	}

	static private void Swap (List<BasicBlock> list, int i, int j)
	{
		BasicBlock tmp = list [i];
		list [i] = list [j];
		list [j] = list [i];
	}
}
