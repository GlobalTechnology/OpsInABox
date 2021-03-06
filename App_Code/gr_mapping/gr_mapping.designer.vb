﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection

Namespace gr_mapping
	
	<Global.System.Data.Linq.Mapping.DatabaseAttribute(Name:="dnn_dev")>  _
	Partial Public Class gr_mappingDataContext
		Inherits System.Data.Linq.DataContext
		
		Private Shared mappingSource As System.Data.Linq.Mapping.MappingSource = New AttributeMappingSource()
		
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub InsertProfilePropertyDefinition(instance As ProfilePropertyDefinition)
    End Sub
    Partial Private Sub UpdateProfilePropertyDefinition(instance As ProfilePropertyDefinition)
    End Sub
    Partial Private Sub DeleteProfilePropertyDefinition(instance As ProfilePropertyDefinition)
    End Sub
    Partial Private Sub InsertAP_StaffBroker_StaffPropertyDefinition(instance As AP_StaffBroker_StaffPropertyDefinition)
    End Sub
    Partial Private Sub UpdateAP_StaffBroker_StaffPropertyDefinition(instance As AP_StaffBroker_StaffPropertyDefinition)
    End Sub
    Partial Private Sub DeleteAP_StaffBroker_StaffPropertyDefinition(instance As AP_StaffBroker_StaffPropertyDefinition)
    End Sub
    Partial Private Sub Insertgr_mapping(instance As gr_mapping)
    End Sub
    Partial Private Sub Updategr_mapping(instance As gr_mapping)
    End Sub
    Partial Private Sub Deletegr_mapping(instance As gr_mapping)
    End Sub
    #End Region
		
		Public Sub New()
			MyBase.New(Global.System.Configuration.ConfigurationManager.ConnectionStrings("SiteSqlServer").ConnectionString, mappingSource)
			OnCreated
		End Sub
		
		Public Sub New(ByVal connection As String)
			MyBase.New(connection, mappingSource)
			OnCreated
		End Sub
		
		Public Sub New(ByVal connection As System.Data.IDbConnection)
			MyBase.New(connection, mappingSource)
			OnCreated
		End Sub
		
		Public Sub New(ByVal connection As String, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
			MyBase.New(connection, mappingSource)
			OnCreated
		End Sub
		
		Public Sub New(ByVal connection As System.Data.IDbConnection, ByVal mappingSource As System.Data.Linq.Mapping.MappingSource)
			MyBase.New(connection, mappingSource)
			OnCreated
		End Sub
		
		Public ReadOnly Property ProfilePropertyDefinitions() As System.Data.Linq.Table(Of ProfilePropertyDefinition)
			Get
				Return Me.GetTable(Of ProfilePropertyDefinition)
			End Get
		End Property
		
		Public ReadOnly Property AP_StaffBroker_StaffPropertyDefinitions() As System.Data.Linq.Table(Of AP_StaffBroker_StaffPropertyDefinition)
			Get
				Return Me.GetTable(Of AP_StaffBroker_StaffPropertyDefinition)
			End Get
		End Property
		
		Public ReadOnly Property gr_mappings() As System.Data.Linq.Table(Of gr_mapping)
			Get
				Return Me.GetTable(Of gr_mapping)
			End Get
		End Property
	End Class
	
	<Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.ProfilePropertyDefinition")>  _
	Partial Public Class ProfilePropertyDefinition
		Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
		
		Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
		
		Private _PropertyDefinitionID As Integer
		
		Private _PortalID As System.Nullable(Of Integer)
		
		Private _ModuleDefID As System.Nullable(Of Integer)
		
		Private _Deleted As Boolean
		
		Private _DataType As Integer
		
		Private _DefaultValue As String
		
		Private _PropertyCategory As String
		
		Private _PropertyName As String
		
		Private _Length As Integer
		
		Private _Required As Boolean
		
		Private _ValidationExpression As String
		
		Private _ViewOrder As Integer
		
		Private _Visible As Boolean
		
		Private _CreatedByUserID As System.Nullable(Of Integer)
		
		Private _CreatedOnDate As System.Nullable(Of Date)
		
		Private _LastModifiedByUserID As System.Nullable(Of Integer)
		
		Private _LastModifiedOnDate As System.Nullable(Of Date)
		
		Private _DefaultVisibility As System.Nullable(Of Integer)
		
		Private _ReadOnly As Boolean
		
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnPropertyDefinitionIDChanging(value As Integer)
    End Sub
    Partial Private Sub OnPropertyDefinitionIDChanged()
    End Sub
    Partial Private Sub OnPortalIDChanging(value As System.Nullable(Of Integer))
    End Sub
    Partial Private Sub OnPortalIDChanged()
    End Sub
    Partial Private Sub OnModuleDefIDChanging(value As System.Nullable(Of Integer))
    End Sub
    Partial Private Sub OnModuleDefIDChanged()
    End Sub
    Partial Private Sub OnDeletedChanging(value As Boolean)
    End Sub
    Partial Private Sub OnDeletedChanged()
    End Sub
    Partial Private Sub OnDataTypeChanging(value As Integer)
    End Sub
    Partial Private Sub OnDataTypeChanged()
    End Sub
    Partial Private Sub OnDefaultValueChanging(value As String)
    End Sub
    Partial Private Sub OnDefaultValueChanged()
    End Sub
    Partial Private Sub OnPropertyCategoryChanging(value As String)
    End Sub
    Partial Private Sub OnPropertyCategoryChanged()
    End Sub
    Partial Private Sub OnPropertyNameChanging(value As String)
    End Sub
    Partial Private Sub OnPropertyNameChanged()
    End Sub
    Partial Private Sub OnLengthChanging(value As Integer)
    End Sub
    Partial Private Sub OnLengthChanged()
    End Sub
    Partial Private Sub OnRequiredChanging(value As Boolean)
    End Sub
    Partial Private Sub OnRequiredChanged()
    End Sub
    Partial Private Sub OnValidationExpressionChanging(value As String)
    End Sub
    Partial Private Sub OnValidationExpressionChanged()
    End Sub
    Partial Private Sub OnViewOrderChanging(value As Integer)
    End Sub
    Partial Private Sub OnViewOrderChanged()
    End Sub
    Partial Private Sub OnVisibleChanging(value As Boolean)
    End Sub
    Partial Private Sub OnVisibleChanged()
    End Sub
    Partial Private Sub OnCreatedByUserIDChanging(value As System.Nullable(Of Integer))
    End Sub
    Partial Private Sub OnCreatedByUserIDChanged()
    End Sub
    Partial Private Sub OnCreatedOnDateChanging(value As System.Nullable(Of Date))
    End Sub
    Partial Private Sub OnCreatedOnDateChanged()
    End Sub
    Partial Private Sub OnLastModifiedByUserIDChanging(value As System.Nullable(Of Integer))
    End Sub
    Partial Private Sub OnLastModifiedByUserIDChanged()
    End Sub
    Partial Private Sub OnLastModifiedOnDateChanging(value As System.Nullable(Of Date))
    End Sub
    Partial Private Sub OnLastModifiedOnDateChanged()
    End Sub
    Partial Private Sub OnDefaultVisibilityChanging(value As System.Nullable(Of Integer))
    End Sub
    Partial Private Sub OnDefaultVisibilityChanged()
    End Sub
    Partial Private Sub OnReadOnlyChanging(value As Boolean)
    End Sub
    Partial Private Sub OnReadOnlyChanged()
    End Sub
    #End Region
		
		Public Sub New()
			MyBase.New
			OnCreated
		End Sub
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PropertyDefinitionID", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=true, IsDbGenerated:=true)>  _
		Public Property PropertyDefinitionID() As Integer
			Get
				Return Me._PropertyDefinitionID
			End Get
			Set
				If ((Me._PropertyDefinitionID = value)  _
							= false) Then
					Me.OnPropertyDefinitionIDChanging(value)
					Me.SendPropertyChanging
					Me._PropertyDefinitionID = value
					Me.SendPropertyChanged("PropertyDefinitionID")
					Me.OnPropertyDefinitionIDChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PortalID", DbType:="Int")>  _
		Public Property PortalID() As System.Nullable(Of Integer)
			Get
				Return Me._PortalID
			End Get
			Set
				If (Me._PortalID.Equals(value) = false) Then
					Me.OnPortalIDChanging(value)
					Me.SendPropertyChanging
					Me._PortalID = value
					Me.SendPropertyChanged("PortalID")
					Me.OnPortalIDChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ModuleDefID", DbType:="Int")>  _
		Public Property ModuleDefID() As System.Nullable(Of Integer)
			Get
				Return Me._ModuleDefID
			End Get
			Set
				If (Me._ModuleDefID.Equals(value) = false) Then
					Me.OnModuleDefIDChanging(value)
					Me.SendPropertyChanging
					Me._ModuleDefID = value
					Me.SendPropertyChanged("ModuleDefID")
					Me.OnModuleDefIDChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Deleted", DbType:="Bit NOT NULL")>  _
		Public Property Deleted() As Boolean
			Get
				Return Me._Deleted
			End Get
			Set
				If ((Me._Deleted = value)  _
							= false) Then
					Me.OnDeletedChanging(value)
					Me.SendPropertyChanging
					Me._Deleted = value
					Me.SendPropertyChanged("Deleted")
					Me.OnDeletedChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_DataType", DbType:="Int NOT NULL")>  _
		Public Property DataType() As Integer
			Get
				Return Me._DataType
			End Get
			Set
				If ((Me._DataType = value)  _
							= false) Then
					Me.OnDataTypeChanging(value)
					Me.SendPropertyChanging
					Me._DataType = value
					Me.SendPropertyChanged("DataType")
					Me.OnDataTypeChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_DefaultValue", DbType:="NText", UpdateCheck:=UpdateCheck.Never)>  _
		Public Property DefaultValue() As String
			Get
				Return Me._DefaultValue
			End Get
			Set
				If (String.Equals(Me._DefaultValue, value) = false) Then
					Me.OnDefaultValueChanging(value)
					Me.SendPropertyChanging
					Me._DefaultValue = value
					Me.SendPropertyChanged("DefaultValue")
					Me.OnDefaultValueChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PropertyCategory", DbType:="NVarChar(50) NOT NULL", CanBeNull:=false)>  _
		Public Property PropertyCategory() As String
			Get
				Return Me._PropertyCategory
			End Get
			Set
				If (String.Equals(Me._PropertyCategory, value) = false) Then
					Me.OnPropertyCategoryChanging(value)
					Me.SendPropertyChanging
					Me._PropertyCategory = value
					Me.SendPropertyChanged("PropertyCategory")
					Me.OnPropertyCategoryChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PropertyName", DbType:="NVarChar(50) NOT NULL", CanBeNull:=false)>  _
		Public Property PropertyName() As String
			Get
				Return Me._PropertyName
			End Get
			Set
				If (String.Equals(Me._PropertyName, value) = false) Then
					Me.OnPropertyNameChanging(value)
					Me.SendPropertyChanging
					Me._PropertyName = value
					Me.SendPropertyChanged("PropertyName")
					Me.OnPropertyNameChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Length", DbType:="Int NOT NULL")>  _
		Public Property Length() As Integer
			Get
				Return Me._Length
			End Get
			Set
				If ((Me._Length = value)  _
							= false) Then
					Me.OnLengthChanging(value)
					Me.SendPropertyChanging
					Me._Length = value
					Me.SendPropertyChanged("Length")
					Me.OnLengthChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Required", DbType:="Bit NOT NULL")>  _
		Public Property Required() As Boolean
			Get
				Return Me._Required
			End Get
			Set
				If ((Me._Required = value)  _
							= false) Then
					Me.OnRequiredChanging(value)
					Me.SendPropertyChanging
					Me._Required = value
					Me.SendPropertyChanged("Required")
					Me.OnRequiredChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ValidationExpression", DbType:="NVarChar(2000)")>  _
		Public Property ValidationExpression() As String
			Get
				Return Me._ValidationExpression
			End Get
			Set
				If (String.Equals(Me._ValidationExpression, value) = false) Then
					Me.OnValidationExpressionChanging(value)
					Me.SendPropertyChanging
					Me._ValidationExpression = value
					Me.SendPropertyChanged("ValidationExpression")
					Me.OnValidationExpressionChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ViewOrder", DbType:="Int NOT NULL")>  _
		Public Property ViewOrder() As Integer
			Get
				Return Me._ViewOrder
			End Get
			Set
				If ((Me._ViewOrder = value)  _
							= false) Then
					Me.OnViewOrderChanging(value)
					Me.SendPropertyChanging
					Me._ViewOrder = value
					Me.SendPropertyChanged("ViewOrder")
					Me.OnViewOrderChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Visible", DbType:="Bit NOT NULL")>  _
		Public Property Visible() As Boolean
			Get
				Return Me._Visible
			End Get
			Set
				If ((Me._Visible = value)  _
							= false) Then
					Me.OnVisibleChanging(value)
					Me.SendPropertyChanging
					Me._Visible = value
					Me.SendPropertyChanged("Visible")
					Me.OnVisibleChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CreatedByUserID", DbType:="Int")>  _
		Public Property CreatedByUserID() As System.Nullable(Of Integer)
			Get
				Return Me._CreatedByUserID
			End Get
			Set
				If (Me._CreatedByUserID.Equals(value) = false) Then
					Me.OnCreatedByUserIDChanging(value)
					Me.SendPropertyChanging
					Me._CreatedByUserID = value
					Me.SendPropertyChanged("CreatedByUserID")
					Me.OnCreatedByUserIDChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_CreatedOnDate", DbType:="DateTime")>  _
		Public Property CreatedOnDate() As System.Nullable(Of Date)
			Get
				Return Me._CreatedOnDate
			End Get
			Set
				If (Me._CreatedOnDate.Equals(value) = false) Then
					Me.OnCreatedOnDateChanging(value)
					Me.SendPropertyChanging
					Me._CreatedOnDate = value
					Me.SendPropertyChanged("CreatedOnDate")
					Me.OnCreatedOnDateChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_LastModifiedByUserID", DbType:="Int")>  _
		Public Property LastModifiedByUserID() As System.Nullable(Of Integer)
			Get
				Return Me._LastModifiedByUserID
			End Get
			Set
				If (Me._LastModifiedByUserID.Equals(value) = false) Then
					Me.OnLastModifiedByUserIDChanging(value)
					Me.SendPropertyChanging
					Me._LastModifiedByUserID = value
					Me.SendPropertyChanged("LastModifiedByUserID")
					Me.OnLastModifiedByUserIDChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_LastModifiedOnDate", DbType:="DateTime")>  _
		Public Property LastModifiedOnDate() As System.Nullable(Of Date)
			Get
				Return Me._LastModifiedOnDate
			End Get
			Set
				If (Me._LastModifiedOnDate.Equals(value) = false) Then
					Me.OnLastModifiedOnDateChanging(value)
					Me.SendPropertyChanging
					Me._LastModifiedOnDate = value
					Me.SendPropertyChanged("LastModifiedOnDate")
					Me.OnLastModifiedOnDateChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_DefaultVisibility", DbType:="Int")>  _
		Public Property DefaultVisibility() As System.Nullable(Of Integer)
			Get
				Return Me._DefaultVisibility
			End Get
			Set
				If (Me._DefaultVisibility.Equals(value) = false) Then
					Me.OnDefaultVisibilityChanging(value)
					Me.SendPropertyChanging
					Me._DefaultVisibility = value
					Me.SendPropertyChanged("DefaultVisibility")
					Me.OnDefaultVisibilityChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Name:="ReadOnly", Storage:="_ReadOnly", DbType:="Bit NOT NULL")>  _
		Public Property [ReadOnly]() As Boolean
			Get
				Return Me._ReadOnly
			End Get
			Set
				If ((Me._ReadOnly = value)  _
							= false) Then
					Me.OnReadOnlyChanging(value)
					Me.SendPropertyChanging
					Me._ReadOnly = value
					Me.SendPropertyChanged("[ReadOnly]")
					Me.OnReadOnlyChanged
				End If
			End Set
		End Property
		
		Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
		
		Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
		
		Protected Overridable Sub SendPropertyChanging()
			If ((Me.PropertyChangingEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
			End If
		End Sub
		
		Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
			If ((Me.PropertyChangedEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub
	End Class
	
	<Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.AP_StaffBroker_StaffPropertyDefinition")>  _
	Partial Public Class AP_StaffBroker_StaffPropertyDefinition
		Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
		
		Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
		
		Private _StaffPropertyDefinitionId As Integer
		
		Private _PropertyName As String
		
		Private _PortalId As System.Nullable(Of Integer)
		
		Private _ViewOrder As System.Nullable(Of Short)
		
		Private _Display As System.Nullable(Of Boolean)
		
		Private _PropertyHelp As String
		
		Private _Type As System.Nullable(Of Byte)
		
		Private _FixedFieldName As String
		
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnStaffPropertyDefinitionIdChanging(value As Integer)
    End Sub
    Partial Private Sub OnStaffPropertyDefinitionIdChanged()
    End Sub
    Partial Private Sub OnPropertyNameChanging(value As String)
    End Sub
    Partial Private Sub OnPropertyNameChanged()
    End Sub
    Partial Private Sub OnPortalIdChanging(value As System.Nullable(Of Integer))
    End Sub
    Partial Private Sub OnPortalIdChanged()
    End Sub
    Partial Private Sub OnViewOrderChanging(value As System.Nullable(Of Short))
    End Sub
    Partial Private Sub OnViewOrderChanged()
    End Sub
    Partial Private Sub OnDisplayChanging(value As System.Nullable(Of Boolean))
    End Sub
    Partial Private Sub OnDisplayChanged()
    End Sub
    Partial Private Sub OnPropertyHelpChanging(value As String)
    End Sub
    Partial Private Sub OnPropertyHelpChanged()
    End Sub
    Partial Private Sub OnTypeChanging(value As System.Nullable(Of Byte))
    End Sub
    Partial Private Sub OnTypeChanged()
    End Sub
    Partial Private Sub OnFixedFieldNameChanging(value As String)
    End Sub
    Partial Private Sub OnFixedFieldNameChanged()
    End Sub
    #End Region
		
		Public Sub New()
			MyBase.New
			OnCreated
		End Sub
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_StaffPropertyDefinitionId", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=true, IsDbGenerated:=true)>  _
		Public Property StaffPropertyDefinitionId() As Integer
			Get
				Return Me._StaffPropertyDefinitionId
			End Get
			Set
				If ((Me._StaffPropertyDefinitionId = value)  _
							= false) Then
					Me.OnStaffPropertyDefinitionIdChanging(value)
					Me.SendPropertyChanging
					Me._StaffPropertyDefinitionId = value
					Me.SendPropertyChanged("StaffPropertyDefinitionId")
					Me.OnStaffPropertyDefinitionIdChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PropertyName", DbType:="VarChar(MAX) NOT NULL", CanBeNull:=false)>  _
		Public Property PropertyName() As String
			Get
				Return Me._PropertyName
			End Get
			Set
				If (String.Equals(Me._PropertyName, value) = false) Then
					Me.OnPropertyNameChanging(value)
					Me.SendPropertyChanging
					Me._PropertyName = value
					Me.SendPropertyChanged("PropertyName")
					Me.OnPropertyNameChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PortalId", DbType:="Int")>  _
		Public Property PortalId() As System.Nullable(Of Integer)
			Get
				Return Me._PortalId
			End Get
			Set
				If (Me._PortalId.Equals(value) = false) Then
					Me.OnPortalIdChanging(value)
					Me.SendPropertyChanging
					Me._PortalId = value
					Me.SendPropertyChanged("PortalId")
					Me.OnPortalIdChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_ViewOrder", DbType:="SmallInt")>  _
		Public Property ViewOrder() As System.Nullable(Of Short)
			Get
				Return Me._ViewOrder
			End Get
			Set
				If (Me._ViewOrder.Equals(value) = false) Then
					Me.OnViewOrderChanging(value)
					Me.SendPropertyChanging
					Me._ViewOrder = value
					Me.SendPropertyChanged("ViewOrder")
					Me.OnViewOrderChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Display", DbType:="Bit")>  _
		Public Property Display() As System.Nullable(Of Boolean)
			Get
				Return Me._Display
			End Get
			Set
				If (Me._Display.Equals(value) = false) Then
					Me.OnDisplayChanging(value)
					Me.SendPropertyChanging
					Me._Display = value
					Me.SendPropertyChanged("Display")
					Me.OnDisplayChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PropertyHelp", DbType:="VarChar(MAX)")>  _
		Public Property PropertyHelp() As String
			Get
				Return Me._PropertyHelp
			End Get
			Set
				If (String.Equals(Me._PropertyHelp, value) = false) Then
					Me.OnPropertyHelpChanging(value)
					Me.SendPropertyChanging
					Me._PropertyHelp = value
					Me.SendPropertyChanged("PropertyHelp")
					Me.OnPropertyHelpChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Type", DbType:="TinyInt")>  _
		Public Property Type() As System.Nullable(Of Byte)
			Get
				Return Me._Type
			End Get
			Set
				If (Me._Type.Equals(value) = false) Then
					Me.OnTypeChanging(value)
					Me.SendPropertyChanging
					Me._Type = value
					Me.SendPropertyChanged("Type")
					Me.OnTypeChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_FixedFieldName", DbType:="NVarChar(100)")>  _
		Public Property FixedFieldName() As String
			Get
				Return Me._FixedFieldName
			End Get
			Set
				If (String.Equals(Me._FixedFieldName, value) = false) Then
					Me.OnFixedFieldNameChanging(value)
					Me.SendPropertyChanging
					Me._FixedFieldName = value
					Me.SendPropertyChanged("FixedFieldName")
					Me.OnFixedFieldNameChanged
				End If
			End Set
		End Property
		
		Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
		
		Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
		
		Protected Overridable Sub SendPropertyChanging()
			If ((Me.PropertyChangingEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
			End If
		End Sub
		
		Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
			If ((Me.PropertyChangedEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub
	End Class
	
	<Global.System.Data.Linq.Mapping.TableAttribute(Name:="dbo.gr_mapping")>  _
	Partial Public Class gr_mapping
		Implements System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
		
		Private Shared emptyChangingEventArgs As PropertyChangingEventArgs = New PropertyChangingEventArgs(String.Empty)
		
		Private _Id As Integer
		
		Private _LocalName As String
		
		Private _gr_dot_notated_name As String
		
		Private _FieldType As String
		
		Private _PortalId As Integer
		
		Private _LocalSource As String
		
		Private _can_be_updated As Boolean
		
		Private _replace As String
		
    #Region "Extensibility Method Definitions"
    Partial Private Sub OnLoaded()
    End Sub
    Partial Private Sub OnValidate(action As System.Data.Linq.ChangeAction)
    End Sub
    Partial Private Sub OnCreated()
    End Sub
    Partial Private Sub OnIdChanging(value As Integer)
    End Sub
    Partial Private Sub OnIdChanged()
    End Sub
    Partial Private Sub OnLocalNameChanging(value As String)
    End Sub
    Partial Private Sub OnLocalNameChanged()
    End Sub
    Partial Private Sub Ongr_dot_notated_nameChanging(value As String)
    End Sub
    Partial Private Sub Ongr_dot_notated_nameChanged()
    End Sub
    Partial Private Sub OnFieldTypeChanging(value As String)
    End Sub
    Partial Private Sub OnFieldTypeChanged()
    End Sub
    Partial Private Sub OnPortalIdChanging(value As Integer)
    End Sub
    Partial Private Sub OnPortalIdChanged()
    End Sub
    Partial Private Sub OnLocalSourceChanging(value As String)
    End Sub
    Partial Private Sub OnLocalSourceChanged()
    End Sub
    Partial Private Sub Oncan_be_updatedChanging(value As Boolean)
    End Sub
    Partial Private Sub Oncan_be_updatedChanged()
    End Sub
    Partial Private Sub OnreplaceChanging(value As String)
    End Sub
    Partial Private Sub OnreplaceChanged()
    End Sub
    #End Region
		
		Public Sub New()
			MyBase.New
			OnCreated
		End Sub
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_Id", AutoSync:=AutoSync.OnInsert, DbType:="Int NOT NULL IDENTITY", IsPrimaryKey:=true, IsDbGenerated:=true)>  _
		Public Property Id() As Integer
			Get
				Return Me._Id
			End Get
			Set
				If ((Me._Id = value)  _
							= false) Then
					Me.OnIdChanging(value)
					Me.SendPropertyChanging
					Me._Id = value
					Me.SendPropertyChanged("Id")
					Me.OnIdChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_LocalName", DbType:="NVarChar(50) NOT NULL", CanBeNull:=false)>  _
		Public Property LocalName() As String
			Get
				Return Me._LocalName
			End Get
			Set
				If (String.Equals(Me._LocalName, value) = false) Then
					Me.OnLocalNameChanging(value)
					Me.SendPropertyChanging
					Me._LocalName = value
					Me.SendPropertyChanged("LocalName")
					Me.OnLocalNameChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_gr_dot_notated_name", DbType:="NVarChar(200) NOT NULL", CanBeNull:=false)>  _
		Public Property gr_dot_notated_name() As String
			Get
				Return Me._gr_dot_notated_name
			End Get
			Set
				If (String.Equals(Me._gr_dot_notated_name, value) = false) Then
					Me.Ongr_dot_notated_nameChanging(value)
					Me.SendPropertyChanging
					Me._gr_dot_notated_name = value
					Me.SendPropertyChanged("gr_dot_notated_name")
					Me.Ongr_dot_notated_nameChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_FieldType", DbType:="VarChar(20) NOT NULL", CanBeNull:=false)>  _
		Public Property FieldType() As String
			Get
				Return Me._FieldType
			End Get
			Set
				If (String.Equals(Me._FieldType, value) = false) Then
					Me.OnFieldTypeChanging(value)
					Me.SendPropertyChanging
					Me._FieldType = value
					Me.SendPropertyChanged("FieldType")
					Me.OnFieldTypeChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_PortalId", DbType:="Int NOT NULL")>  _
		Public Property PortalId() As Integer
			Get
				Return Me._PortalId
			End Get
			Set
				If ((Me._PortalId = value)  _
							= false) Then
					Me.OnPortalIdChanging(value)
					Me.SendPropertyChanging
					Me._PortalId = value
					Me.SendPropertyChanged("PortalId")
					Me.OnPortalIdChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_LocalSource", DbType:="VarChar(2) NOT NULL", CanBeNull:=false)>  _
		Public Property LocalSource() As String
			Get
				Return Me._LocalSource
			End Get
			Set
				If (String.Equals(Me._LocalSource, value) = false) Then
					Me.OnLocalSourceChanging(value)
					Me.SendPropertyChanging
					Me._LocalSource = value
					Me.SendPropertyChanged("LocalSource")
					Me.OnLocalSourceChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_can_be_updated", DbType:="Bit NOT NULL")>  _
		Public Property can_be_updated() As Boolean
			Get
				Return Me._can_be_updated
			End Get
			Set
				If ((Me._can_be_updated = value)  _
							= false) Then
					Me.Oncan_be_updatedChanging(value)
					Me.SendPropertyChanging
					Me._can_be_updated = value
					Me.SendPropertyChanged("can_be_updated")
					Me.Oncan_be_updatedChanged
				End If
			End Set
		End Property
		
		<Global.System.Data.Linq.Mapping.ColumnAttribute(Storage:="_replace", DbType:="NVarChar(MAX)")>  _
		Public Property replace() As String
			Get
				Return Me._replace
			End Get
			Set
				If (String.Equals(Me._replace, value) = false) Then
					Me.OnreplaceChanging(value)
					Me.SendPropertyChanging
					Me._replace = value
					Me.SendPropertyChanged("replace")
					Me.OnreplaceChanged
				End If
			End Set
		End Property
		
		Public Event PropertyChanging As PropertyChangingEventHandler Implements System.ComponentModel.INotifyPropertyChanging.PropertyChanging
		
		Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
		
		Protected Overridable Sub SendPropertyChanging()
			If ((Me.PropertyChangingEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanging(Me, emptyChangingEventArgs)
			End If
		End Sub
		
		Protected Overridable Sub SendPropertyChanged(ByVal propertyName As [String])
			If ((Me.PropertyChangedEvent Is Nothing)  _
						= false) Then
				RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
			End If
		End Sub
	End Class
End Namespace
