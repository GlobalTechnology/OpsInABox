Param(
  [string]$webConfig
)
# $webConfig = "C:\aDev\acDev1\web.config"
   $doc = new-object System.Xml.XmlDocument
   $doc.Load($webConfig)
   
   $codeSubDirectories = $doc.SelectSingleNode('//system.web/compilation/codeSubDirectories')
   
   if(-not $codeSubDirectories)
   {
   $compilation = $doc.SelectSingleNode("//system.web/compilation")
   $codeSubDirectories = $doc.CreateElement("codeSubDirectories")
   $compilation.AppendChild($codeSubDirectories)
   }
   
   $exists = "NO"
    FOREACH ($j in $codeSubDirectories.ChildNodes)
	{
		if ($j.directoryName -eq "TheKey"){$exists="YES";}
   } 
   if($exists -eq "NO")
   {
	$theKey = $doc.CreateElement("add")
	$xmlAttr1 = $doc.CreateAttribute("directoryName")
	$xmlAttr1.Value = "TheKey"
	$theKey.Attributes.Append($xmlAttr1)
	$codeSubDirectories.AppendChild($theKey)
   }
    $exists = "NO"
    FOREACH ($j in $codeSubDirectories.ChildNodes)
	{
		if ($j.directoryName -eq "StaffBroker"){$exists="YES";}
   } 
   if($exists -eq "NO")
   {
   $staffBroker = $doc.CreateElement("add")
   $xmlAttr2 = $doc.CreateAttribute("directoryName")
   $xmlAttr2.Value = "StaffBroker"
   $staffBroker.Attributes.Append($xmlAttr2)
   $codeSubDirectories.AppendChild($staffBroker)
   }
  
    $exists = "NO"
    FOREACH ($j in $codeSubDirectories.ChildNodes)
	{
		if ($j.directoryName -eq "tntWebUsers"){$exists="YES";}
   } 
   if($exists -eq "NO")
   {
   $tntWebUsers = $doc.CreateElement("add")
   $xmlAttr4 = $doc.CreateAttribute("directoryName")
   $xmlAttr4.Value = "tntWebUsers"
   $tntWebUsers.Attributes.Append($xmlAttr4)
   $codeSubDirectories.AppendChild($tntWebUsers)
   }
   
   
  
  $doc.Save($webConfig)