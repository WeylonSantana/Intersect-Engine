namespace Intersect.Server.CustomChange.SCFVHub;

public class SCFVConfig
{
    /// <summary>
    /// This property tell to the app if the SCFV is open or not
    /// </summary>
    public bool IsOpen { get; set; } = false;
}

public enum SCFVUserType
{
    Morning,
    Afternoon,
    Night,
}

public class SCFVUser
{
    /// <summary>
    /// Id of the worker who is using the SCFV
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// Name of the student/user
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Last modified date
    /// </summary>
    public string LastModified { get; set; }
}
