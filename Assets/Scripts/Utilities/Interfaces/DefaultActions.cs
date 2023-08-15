using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDefaultActions
{
    void PerformAttack();

    void PerformRanged();

    void PerformHeal();

    void PerformUltimate();
}
