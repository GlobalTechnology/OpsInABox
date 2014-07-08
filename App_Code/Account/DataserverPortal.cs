using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DataserverPortal
/// </summary>
public class DataserverPortal
{
    private string _instanceUri;
    private string _instanceName;

	public DataserverPortal()
	{
	
	}

    public DataserverPortal(string instanceUri, string instanceName)
    {
        _instanceUri = instanceUri;
        _instanceName = instanceName;
    }

    public string InstanceUri
    {
        get { return _instanceUri; }
        set { _instanceUri = value; }
    }

    public string InstanceName
    {
        get { return _instanceName; }
        set { _instanceName = value; }
    }
}