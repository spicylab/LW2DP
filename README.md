LW2DP
====
<b>Lightweight 2D Platformer Character Controller</b><br>
<br>
Performance-first, Less physics dependent 2d controller with minimalized feature set.

Comparision with Corgi Engine
----
Corgi casts `8x2` rays for every frame to make more precise movement. You can reduce the number of rays, but they still casts ray in every frame and lead character to wrong behavior.<br>
Even worse, they have ability system to make character jump, wander or dash. Each abilities is made with CBD style. 
This may look cool and make code nicer and short, but horrible for performances.<br>
Most of abilities do more physics calculations for perform their action even if they are already done by other components.

Performance Comparision
----
I found that 50 corgi characters take __20ms__ to finish their movements which is actually huge for 60fps program.<br>
Here are more detailed comparision result between `LW2DP` and `Corgi Engine`.

Is this a replacement of Corgi?
----
No, This is not aimed to replace the Corgi. 
