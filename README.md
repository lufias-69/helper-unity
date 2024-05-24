
# Helper Tools

This tool is designed to make the development phase easier for Unity developers by providing various extensions, custom attributes and editor menu options.
## Features

- **Transform Extensions:** Additional functionality for transform operations.
- **Custom Attributes:**
    - `ButtonLUFI`: Easily create buttons in the inspector.
    - `ShowIf`: Conditionally display fields in the inspector. You can check condition with bool, int, float and enum
    - `ReadOnlyLUFI`: Make fields read-only in the inspector.
- **Editor Menu Options:**
    - **Import/Remove assets**
    - **Remove Missing Scripts:** Clean up all missing scripts from the scene with one click.
    - **And Many More:** Various other tools to enhance your Unity development experience.
## Installation

To add Helper Tool, follow these steps:
* Open your Unity project.
* Go to `Window` > `Package Manager`.
* Click on the `+` button and select `Add package from git URL`.
* Enter the following GitHub link:
```bash
  https://github.com/lufias-69/helper-unity.git
```
Click Add and wait for the package to be imported.
## Usage/Examples


```csharp
public class Temp : MonoBehaviour
{
    [ReadOnlyLUFI]
    public int a = 123;

    public bool b;
    [ShowIf("b")] public int a;

    [ButtonLUFI]
    void MyFunc() { }
}
```

```csharp
using Helper.Extension;
using UnityEngine;

public class Test : MonoBehaviour
{
    void OldWayTransform()
    {
        Vector3 pos = transform.position;
        pos.y = 5;
        transform.position = pos;
    }

    void NewWayTransform()
    {
        transform.SetPosY(5);
    }

    void OldWayChildKill()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (Application.isPlaying) Destroy(transform.GetChild(i).gameObject);
            else DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    void NewWayChildKill()
    {
        transform.KillAllChild();
    }
}
```

```csharp
using Helper.Waiter;
using UnityEngine;

public class Test : MonoBehaviour
{
    void DelayCallExample()
    {
        //execules now

        Waiter.Wait(5f, () =>
        {
            //will execute 5s later
        });
    }
}
```



## Feedback

If you have any questions or feedback, feel free to reach out:

dev_lufias@icloud.com 

