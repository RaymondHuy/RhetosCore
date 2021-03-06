﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rhetos.DatabaseGenerator.DefaultConcepts {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Sql_MsSql {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Sql_MsSql() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Rhetos.DatabaseGenerator.DefaultConcepts.Sql.MsSql", typeof(Sql_MsSql).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to VARBINARY(MAX).
        /// </summary>
        internal static string BinaryPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("BinaryPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BIT.
        /// </summary>
        internal static string BoolPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("BoolPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///SET NOCOUNT ON
        ///
        ///UPDATE
        ///	newData
        ///SET
        ///	{2} = GETDATE()
        ///FROM
        ///	{0}.{1} newData
        ///	INNER JOIN inserted ON inserted.ID = newData.ID
        ///WHERE
        ///	inserted.{2} IS NULL
        ///.
        /// </summary>
        internal static string CreationTimeDatabaseDefinition_TriggerBody {
            get {
                return ResourceManager.GetString("CreationTimeDatabaseDefinition_TriggerBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FK_{0}_{1}_ID.
        /// </summary>
        internal static string DataStructureExtendsDatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("DataStructureExtendsDatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0} ADD CONSTRAINT {1} FOREIGN KEY (ID) REFERENCES {2} (ID) ON DELETE CASCADE {3};.
        /// </summary>
        internal static string DataStructureExtendsDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("DataStructureExtendsDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0} DROP CONSTRAINT {1};.
        /// </summary>
        internal static string DataStructureExtendsDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("DataStructureExtendsDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DATE.
        /// </summary>
        internal static string DatePropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("DatePropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DATETIME.
        /// </summary>
        internal static string DateTimePropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("DateTimePropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECIMAL(28,10).
        /// </summary>
        internal static string DecimalPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("DecimalPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE {0}.{1} (ID UNIQUEIDENTIFIER NOT NULL CONSTRAINT {2} PRIMARY KEY NONCLUSTERED CONSTRAINT {3} DEFAULT (NEWID()));
        ///EXEC Rhetos.DataMigrationApply &apos;{0}&apos;, &apos;{1}&apos;, &apos;ID&apos;;.
        /// </summary>
        internal static string EntityDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("EntityDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DF_{1}_ID.
        /// </summary>
        internal static string EntityDatabaseDefinition_DefaultConstraintName {
            get {
                return ResourceManager.GetString("EntityDatabaseDefinition_DefaultConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PK_{1}.
        /// </summary>
        internal static string EntityDatabaseDefinition_PrimaryKeyConstraintName {
            get {
                return ResourceManager.GetString("EntityDatabaseDefinition_PrimaryKeyConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EXEC Rhetos.DataMigrationUse &apos;{0}&apos;, &apos;{1}&apos;, &apos;ID&apos;, NULL;
        ///DROP TABLE {0}.{1};.
        /// </summary>
        internal static string EntityDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("EntityDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TRIGGER {0}.{2} ON {0}.{1}
        ///FOR UPDATE, DELETE, INSERT
        ///AS
        ///BEGIN
        ///SET NOCOUNT ON
        ///IF NOT EXISTS (SELECT TOP 1 1 FROM inserted UNION ALL SELECT TOP 1 1 FROM deleted) RETURN
        ///
        ///CREATE TABLE #log
        ///(
        ///    ID uniqueidentifier NOT NULL,
        ///    Action nvarchar(256),
        ///    TableName nvarchar(256),
        ///    ItemId uniqueidentifier,
        ///    Description nvarchar(max){7}
        ///);
        ///
        ///INSERT INTO
        ///    #log
        ///    (
        ///        ID,
        ///        Action,
        ///        TableName,
        ///        ItemId,
        ///        Description{8}
        ///    )
        ///SELECT
        ///    ID =  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EntityLoggingDefinition_Create {
            get {
                return ResourceManager.GetString("EntityLoggingDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TRIGGER {0}.{1};.
        /// </summary>
        internal static string EntityLoggingDefinition_Remove {
            get {
                return ResourceManager.GetString("EntityLoggingDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{1}_Logging.
        /// </summary>
        internal static string EntityLoggingDefinition_TriggerNameDelete {
            get {
                return ResourceManager.GetString("EntityLoggingDefinition_TriggerNameDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{1}_Logging.
        /// </summary>
        internal static string EntityLoggingDefinition_TriggerNameInsert {
            get {
                return ResourceManager.GetString("EntityLoggingDefinition_TriggerNameInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{1}_Logging.
        /// </summary>
        internal static string EntityLoggingDefinition_TriggerNameUpdate {
            get {
                return ResourceManager.GetString("EntityLoggingDefinition_TriggerNameUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UNIQUEIDENTIFIER.
        /// </summary>
        internal static string GuidPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("GuidPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INTEGER.
        /// </summary>
        internal static string IntegerPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("IntegerPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE VIEW {0}.{1} WITH VIEW_METADATA AS
        ///SELECT
        ///    source.ID{7}
        ///FROM
        ///    {3} source
        ///{8};
        ///
        ///{2}
        ///
        ///CREATE TRIGGER {0}.{4}
        ///ON {0}.{1} INSTEAD OF INSERT
        ///AS
        ///IF NOT EXISTS(SELECT * FROM inserted) RETURN;
        ///SET NOCOUNT ON;
        ///
        ///INSERT INTO
        ///    {3} (ID{9})
        ///SELECT
        ///    inserted.ID{10}
        ///FROM
        ///    inserted
        ///{12};
        ///
        ///{2}
        ///
        ///CREATE TRIGGER {0}.{5}
        ///ON {0}.{1} INSTEAD OF UPDATE
        ///AS
        ///IF NOT EXISTS(SELECT * FROM inserted) RETURN;
        ///SET NOCOUNT ON;
        ///
        ///DECLARE @dummy INT
        ///
        ///UPDATE
        ///    source
        ///SET
        ///    @dummy = 0{1 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LegacyEntityWithAutoCreatedViewDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("LegacyEntityWithAutoCreatedViewDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{0}_LegacyDelete.
        /// </summary>
        internal static string LegacyEntityWithAutoCreatedViewDatabaseDefinition_DeleteTriggerName {
            get {
                return ResourceManager.GetString("LegacyEntityWithAutoCreatedViewDatabaseDefinition_DeleteTriggerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{0}_LegacyInsert.
        /// </summary>
        internal static string LegacyEntityWithAutoCreatedViewDatabaseDefinition_InsertTriggerName {
            get {
                return ResourceManager.GetString("LegacyEntityWithAutoCreatedViewDatabaseDefinition_InsertTriggerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TRIGGER {0}.{4};
        ///DROP TRIGGER {0}.{3};
        ///DROP TRIGGER {0}.{2};
        ///DROP VIEW {0}.{1};.
        /// </summary>
        internal static string LegacyEntityWithAutoCreatedViewDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("LegacyEntityWithAutoCreatedViewDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{0}_LegacyUpdate.
        /// </summary>
        internal static string LegacyEntityWithAutoCreatedViewDatabaseDefinition_UpdateTriggerName {
            get {
                return ResourceManager.GetString("LegacyEntityWithAutoCreatedViewDatabaseDefinition_UpdateTriggerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = source.{1}.
        /// </summary>
        internal static string LegacyPropertyReadOnlyDatabaseDefinition_ViewSelect {
            get {
                return ResourceManager.GetString("LegacyPropertyReadOnlyDatabaseDefinition_ViewSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}.{1} = source.{2}.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendFromJoin {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendFromJoin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     LEFT JOIN {0} {1} ON {1}.ID = inserted.{2}ID
        ///.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerFrom {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to , {0}.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerInsert {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = {1}.{2}.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerSelectForInsert {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerSelectForInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = {1}.{2}.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerSelectForUpdate {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendTriggerSelectForUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to     LEFT JOIN {0} {1} ON {2}
        ///.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendViewFrom {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendViewFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0}ID = {1}.ID.
        /// </summary>
        internal static string LegacyPropertyReferenceDatabaseDefinition_ExtendViewSelect {
            get {
                return ResourceManager.GetString("LegacyPropertyReferenceDatabaseDefinition_ExtendViewSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to , {0}.
        /// </summary>
        internal static string LegacyPropertySimpleDatabaseDefinition_ExtendTriggerInsert {
            get {
                return ResourceManager.GetString("LegacyPropertySimpleDatabaseDefinition_ExtendTriggerInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = inserted.{1}.
        /// </summary>
        internal static string LegacyPropertySimpleDatabaseDefinition_ExtendTriggerSelectForInsert {
            get {
                return ResourceManager.GetString("LegacyPropertySimpleDatabaseDefinition_ExtendTriggerSelectForInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = inserted.{1}.
        /// </summary>
        internal static string LegacyPropertySimpleDatabaseDefinition_ExtendTriggerSelectForUpdate {
            get {
                return ResourceManager.GetString("LegacyPropertySimpleDatabaseDefinition_ExtendTriggerSelectForUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = source.{1}.
        /// </summary>
        internal static string LegacyPropertySimpleDatabaseDefinition_ExtendViewSelect {
            get {
                return ResourceManager.GetString("LegacyPropertySimpleDatabaseDefinition_ExtendViewSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO
        ///    Common.LogRelatedItem (ID, LogID, TableName, ItemId, Relation)
        ///SELECT
        ///    ID = NEWID(),
        ///    LogID = ID,
        ///    TableName = &apos;{2}&apos;,
        ///    ItemId = {0},
        ///    Relation = {4}
        ///FROM
        ///    #log
        ///WHERE
        ///    {0} IS NOT NULL;
        ///
        ///INSERT INTO
        ///    Common.LogRelatedItem (ID, LogID, TableName, ItemId, Relation)
        ///SELECT
        ///    ID = NEWID(),
        ///    LogID = ID,
        ///    TableName = &apos;{2}&apos;,
        ///    ItemId = {1},
        ///    Relation = {4}
        ///FROM
        ///    #log
        ///WHERE
        ///    {1} IS NOT NULL
        ///    AND ({1} &lt;&gt; {0} OR {0} IS NULL);
        ///
        ///.
        /// </summary>
        internal static string LoggingRelatedItemDatabaseDefinition_AfterInsertLog {
            get {
                return ResourceManager.GetString("LoggingRelatedItemDatabaseDefinition_AfterInsertLog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} uniqueidentifier,
        ///    {1} uniqueidentifier.
        /// </summary>
        internal static string LoggingRelatedItemDatabaseDefinition_TempColumnDefinition {
            get {
                return ResourceManager.GetString("LoggingRelatedItemDatabaseDefinition_TempColumnDefinition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///        {0},
        ///        {1}.
        /// </summary>
        internal static string LoggingRelatedItemDatabaseDefinition_TempColumnList {
            get {
                return ResourceManager.GetString("LoggingRelatedItemDatabaseDefinition_TempColumnList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ,
        ///    {0} = deleted.{3},
        ///    {1} = inserted.{3}.
        /// </summary>
        internal static string LoggingRelatedItemDatabaseDefinition_TempColumnSelect {
            get {
                return ResourceManager.GetString("LoggingRelatedItemDatabaseDefinition_TempColumnSelect", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NVARCHAR(MAX).
        /// </summary>
        internal static string LongStringPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("LongStringPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///SET NOCOUNT ON
        ///
        ///UPDATE
        ///	newData
        ///SET
        ///	{2} = GETDATE()
        ///FROM
        ///	{0}.{1} newData
        ///	INNER JOIN inserted ON inserted.ID = newData.ID
        ///WHERE
        ///	inserted.{2} IS NULL
        ///.
        /// </summary>
        internal static string ModificationTimeOfDatabaseDefinition_InsertTriggerBody {
            get {
                return ResourceManager.GetString("ModificationTimeOfDatabaseDefinition_InsertTriggerBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///SET NOCOUNT ON
        ///
        ///IF UPDATE({3})
        ///BEGIN
        ///  UPDATE
        ///    newData
        ///  SET
        ///    {2} = GETDATE()
        ///  FROM
        ///    {0}.{1} newData
        ///    INNER JOIN inserted ON inserted.ID = newData.ID
        ///    INNER JOIN deleted ON deleted.ID = inserted.ID
        ///  WHERE
        ///    inserted.{3} &lt;&gt; deleted.{3}
        ///    OR inserted.{3} IS NULL AND deleted.{3} IS NOT NULL
        ///    OR inserted.{3} IS NOT NULL AND deleted.{3} IS NULL
        ///END
        ///.
        /// </summary>
        internal static string ModificationTimeOfDatabaseDefinition_UpdateTriggerBody {
            get {
                return ResourceManager.GetString("ModificationTimeOfDatabaseDefinition_UpdateTriggerBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE SCHEMA {0}.
        /// </summary>
        internal static string ModuleDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("ModuleDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP SCHEMA {0}.
        /// </summary>
        internal static string ModuleDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("ModuleDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CK_{0}_{1}_money.
        /// </summary>
        internal static string MoneyPropertyDatabaseDefinition_CheckConstraintName {
            get {
                return ResourceManager.GetString("MoneyPropertyDatabaseDefinition_CheckConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CONSTRAINT {0} CHECK({1} IS NULL OR {1} = ROUND({1}, 2)).
        /// </summary>
        internal static string MoneyPropertyDatabaseDefinition_CreateCheckConstraint {
            get {
                return ResourceManager.GetString("MoneyPropertyDatabaseDefinition_CreateCheckConstraint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MONEY.
        /// </summary>
        internal static string MoneyPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("MoneyPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0}.{1} DROP CONSTRAINT {2};.
        /// </summary>
        internal static string MoneyPropertyDatabaseDefinition_RemoveCheckConstraint {
            get {
                return ResourceManager.GetString("MoneyPropertyDatabaseDefinition_RemoveCheckConstraint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///ALTER TABLE {0}.{1} ADD {2} {3} {4} {5} {6};
        ///EXEC Rhetos.DataMigrationApply &apos;{0}&apos;, &apos;{1}&apos;, &apos;{2}&apos;;
        ///{7}
        ///    .
        /// </summary>
        internal static string PropertyDatabaseDefinition_AddColumn {
            get {
                return ResourceManager.GetString("PropertyDatabaseDefinition_AddColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///{3}
        ///EXEC Rhetos.DataMigrationUse &apos;{0}&apos;, &apos;{1}&apos;, &apos;{2}&apos;, NULL;
        ///ALTER TABLE {0}.{1} DROP COLUMN [{2}]
        ///    .
        /// </summary>
        internal static string PropertyDatabaseDefinition_RemoveColumn {
            get {
                return ResourceManager.GetString("PropertyDatabaseDefinition_RemoveColumn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to + CASE WHEN inserted.{0} &lt;&gt; deleted.{0}
        ///                    OR inserted.{0} IS NULL AND deleted.{0} IS NOT NULL
        ///                    OR inserted.{0} IS NOT NULL AND deleted.{0} IS NULL
        ///                THEN &apos; {0}=&quot;&apos; + REPLACE(REPLACE(REPLACE(REPLACE(ISNULL({1}, &apos;&apos;), &apos;&amp;&apos;, &apos;&amp;amp;&apos;), &apos;&lt;&apos;, &apos;&amp;lt;&apos;), &apos;&gt;&apos;, &apos;&amp;gt;&apos;), &apos;&quot;&apos;, &apos;&amp;quot;&apos;) + &apos;&quot;&apos;
        ///                ELSE &apos;&apos; END
        ///			.
        /// </summary>
        internal static string PropertyLoggingDefinition_GenericPropertyLogging {
            get {
                return ResourceManager.GetString("PropertyLoggingDefinition_GenericPropertyLogging", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CONVERT(NVARCHAR(MAX), deleted.{0}, 126).
        /// </summary>
        internal static string PropertyLoggingDefinition_TextValue {
            get {
                return ResourceManager.GetString("PropertyLoggingDefinition_TextValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CONVERT(NVARCHAR(MAX), deleted.{0}, 1).
        /// </summary>
        internal static string PropertyLoggingDefinition_TextValue_Binary {
            get {
                return ResourceManager.GetString("PropertyLoggingDefinition_TextValue_Binary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to deleted.{0}.
        /// </summary>
        internal static string PropertyLoggingDefinition_TextValue_LongString {
            get {
                return ResourceManager.GetString("PropertyLoggingDefinition_TextValue_LongString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to deleted.{0}.
        /// </summary>
        internal static string PropertyLoggingDefinition_TextValue_ShortString {
            get {
                return ResourceManager.GetString("PropertyLoggingDefinition_TextValue_ShortString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  ON DELETE CASCADE .
        /// </summary>
        internal static string ReferenceCascadeDeleteDatabaseDefinition_ExtendForeignKey {
            get {
                return ResourceManager.GetString("ReferenceCascadeDeleteDatabaseDefinition_ExtendForeignKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FK_{0}_{1}_{2}ID.
        /// </summary>
        internal static string ReferencePropertyConstraintDatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("ReferencePropertyConstraintDatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0} ADD CONSTRAINT {1} FOREIGN KEY ({2}) REFERENCES {3} (ID) {4}.
        /// </summary>
        internal static string ReferencePropertyConstraintDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("ReferencePropertyConstraintDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0} DROP CONSTRAINT {1}.
        /// </summary>
        internal static string ReferencePropertyConstraintDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("ReferencePropertyConstraintDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UNIQUEIDENTIFIER.
        /// </summary>
        internal static string ReferencePropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("ReferencePropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NVARCHAR({0}).
        /// </summary>
        internal static string ShortStringPropertyDatabaseDefinition_DataType {
            get {
                return ResourceManager.GetString("ShortStringPropertyDatabaseDefinition_DataType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DF_{0}_{1}.
        /// </summary>
        internal static string SqlDefaultPropertyDatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("SqlDefaultPropertyDatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0}.{1} ADD CONSTRAINT {3} DEFAULT ({4}) FOR {2};.
        /// </summary>
        internal static string SqlDefaultPropertyDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlDefaultPropertyDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE {0}.{1} DROP CONSTRAINT {3};.
        /// </summary>
        internal static string SqlDefaultPropertyDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlDefaultPropertyDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///CREATE FUNCTION {0}.{1} ({2})
        ///{3}
        ///.
        /// </summary>
        internal static string SqlFunctionDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlFunctionDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP FUNCTION {0}.{1}.
        /// </summary>
        internal static string SqlFunctionDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlFunctionDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IX_{0}_{1}_{2}.
        /// </summary>
        internal static string SqlIndex2DatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("SqlIndex2DatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IX_{0}_{1}_{2}_{3}.
        /// </summary>
        internal static string SqlIndex3DatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("SqlIndex3DatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IX_{0}_{1}.
        /// </summary>
        internal static string SqlIndexDatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("SqlIndexDatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IX_{0}_{1}.
        /// </summary>
        internal static string SqlIndexMultipleDatabaseDefinition_ConstraintName {
            get {
                return ResourceManager.GetString("SqlIndexMultipleDatabaseDefinition_ConstraintName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE {4} INDEX {0} ON {1}.{2} ({3}) {5}.
        /// </summary>
        internal static string SqlIndexMultipleDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlIndexMultipleDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP INDEX {0}.{1}.{2}.
        /// </summary>
        internal static string SqlIndexMultipleDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlIndexMultipleDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///{5}
        ///UPDATE {0}.{1} SET {2} = {4} WHERE {2} IS NULL;
        ///ALTER TABLE {0}.{1} ALTER COLUMN {2} {3} NOT NULL;
        ///    .
        /// </summary>
        internal static string SqlNotNull_Create {
            get {
                return ResourceManager.GetString("SqlNotNull_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE PROCEDURE {0}.{1}
        ///{2}
        ///AS
        ///{3}
        ///.
        /// </summary>
        internal static string SqlProcedureDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlProcedureDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP PROCEDURE {0}.{1}.
        /// </summary>
        internal static string SqlProcedureDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlProcedureDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE VIEW {0}.{1} AS {2}.
        /// </summary>
        internal static string SqlQueryableDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlQueryableDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP VIEW {0}.{1}.
        /// </summary>
        internal static string SqlQueryableDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlQueryableDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TRIGGER {0}.{1}
        ///ON {0}.{2} {3}
        ///AS
        ///SET NOCOUNT ON
        ///IF (NOT EXISTS(SELECT * FROM inserted)) AND (NOT EXISTS(SELECT * FROM deleted)) RETURN;
        ///{4}.
        /// </summary>
        internal static string SqlTriggerDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlTriggerDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP TRIGGER {0}.{1}.
        /// </summary>
        internal static string SqlTriggerDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlTriggerDatabaseDefinition_Remove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to trg_{0}_{1}.
        /// </summary>
        internal static string SqlTriggerDatabaseDefinition_TriggerName {
            get {
                return ResourceManager.GetString("SqlTriggerDatabaseDefinition_TriggerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UNIQUE .
        /// </summary>
        internal static string SqlUniqueMultipleDatabaseDefinition_ExtendOption1 {
            get {
                return ResourceManager.GetString("SqlUniqueMultipleDatabaseDefinition_ExtendOption1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE VIEW {0}.{1}
        ///AS
        ///{2}
        ///.
        /// </summary>
        internal static string SqlViewDatabaseDefinition_Create {
            get {
                return ResourceManager.GetString("SqlViewDatabaseDefinition_Create", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DROP VIEW {0}.{1}.
        /// </summary>
        internal static string SqlViewDatabaseDefinition_Remove {
            get {
                return ResourceManager.GetString("SqlViewDatabaseDefinition_Remove", resourceCulture);
            }
        }
    }
}
