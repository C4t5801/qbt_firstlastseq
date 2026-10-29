======================
Configure qBittorrent:
======================
----------
* STEP 1 *
----------

_Top Menu Bar:

_Tools => Options => WebUI

_Set/Enable:   [v] Web User Interface (Remote control)

_Set:              IP adress: 127.0.0.1 (localhost) 
                   Port: 8080 (or custom value)

_Under 'Authentication' :

_Set:              Username: admin 
_Set:              Password: 123456

_Set/Enable:   [v] Bypass authentication for clients on localhost

----------
* STEP 2 *
----------

_Tools => Options => Downloads

_Under 'Run external program' :

_Set/Enable:  [v] Run on torrent added:

_Now insert the following command:
 
"c:\Program Files\qBittorrent\qbt_firstlastseq.exe" "%K"

(This will use the default port: 8080)


============
* OPTIONAL * 
============

----
PORT
----

You can add a custom port using "-pX"  (X=Portnumber)

"c:\Program Files\qBittorrent\qbt_firstlastseq.exe" "%K" "-p9095" 

----
HIDE
----

For hidden/silent mode (only show on error) add "-hide"

"c:\Program Files\qBittorrent\qbt_firstlastseq.exe" "%K" "-p9095" "-hide"

Or when using the default port:

"c:\Program Files\qBittorrent\qbt_firstlastseq.exe" "%K" "-hide" 


==================================================
Initial code by Google AI, Prompt/Adjustments: C4T 
==================================================




