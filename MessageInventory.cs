
using Amazon.DynamoDBv2.DataModel;

/// <summary>
/// PK				SK
/// USER#username	#METADATA#username	username
/// GROUP#groupname	#METADATA#groupname	groupname
/// GROUP#groupname	USER#username		username	groupname
/// MSG#groupname	TIME#time			groupname	postedat	username	msg
/// </summary>
[DynamoDBTable("MessageInventory")]
class MessageInventory
{
    [DynamoDBHashKey]
    public string PK { get; set; }
    [DynamoDBRangeKey]
    public string SK { get; set; }
}