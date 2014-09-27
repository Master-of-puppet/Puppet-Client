Puppet Client C#
================

The first prototype of Puppet SSDK project

Directory structure
-------------------

Before the build DLL for Unity3D. Please add "USE_UNITY" to "Conditional compilation symbols"
If you use PuppetClient.dll for Unity3D, please add "USE_UNITY" to "Scripting Define Symbols" in Build Setting

For Use
--------

Example:
```
PuMain.Instance.Load(); 
```
Or customized settings before using
```
PuMain.Setting = new class MySetting : IPuSetting;
```

<b>Full Sample</b>
```
public class Example : MonoBehaviour
{
    void Awake()
    {
        PuMain.Instance.Load();
    }

    void FixedUpdate()
    {
        if (Puppet.PuMain.Setting.ActionUpdate != null)
            Puppet.PuMain.Setting.ActionUpdate();
    }
}
```



Dependency library
------------------
Unity
-----
- Loom, http://unitygems.com/threads/
- MiniJSON, https://gist.github.com/darktable/1411710
- LitJson, http://lbv.github.io/litjson/ (LitJSON 0.9.0)
- SmartFox2X, http://www.smartfoxserver.com/download/sfs2x#p=client