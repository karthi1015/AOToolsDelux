﻿namespace AOTools
{
	public enum ERevDataItems2
	{
		// these enums are what is stored in the items collection
		REV_SELECTED = 0,         // (derived) flag that this data item has 
		//                        // been selected
		REV_SEQ,                  // (from sequence) revision sequence number
		//                        // for ordering only
		REV_KEY_ALTID,            // (from issued by) (part of item key) 
		//                        // a cross-reference to the REV_REVID associated with this item
		REV_KEY_TYPE_CODE,        // (derived) (part of item key) code based  
		//                        // on the document type
		REV_KEY_DISCIPLINE_CODE,  // (derived) (part of item key) code based
		//                        // on the discipline
		REV_KEY_DELTA_TITLE,      // (from issued to) (part of item key) 
		//                        // simple name for this issuance (goes below the delta)
		REV_KEY_SHTNUM,           // (calculated) (part of item key) sheet 
		//                        // number of this tag
		REV_ITEM_VISIBLE,         // (from visibility)(calculated)) item 
		//                        // visibility
		REV_ITEM_REVID,           // (from revision) revision id number or 
		//                        // alphanumeric
		REV_ITEM_BLOCK_TITLE,     // (from revision description) title for 
		//                        // this issuance
		REV_ITEM_DATE,            // (from revision date) the date assigned 
		//                        // to the revision
		REV_ITEM_BASIS,           // (from comment) the reason for the 
		//                        // revision
		REV_ITEM_DESC,            // (from mark) the description of the 
		//                        // revision
		REV_TAG_ELEM_ID,          // the element id of the tag for this 
		//                        // data item
		REV_CLOUD_ELEM_ID,        // the element id of of the cloud for 
		//                        // this data item
		REV_ITEMS_LEN,            // the total number of items

		// management items - these enums are not stored in the items collection
		REV_MGMT_COLUMN = -1,     // (derived) the title for the column field 
		REV_MGMT_LEN = 1,         // number of management items

		// control items
		REV_CTRL_INVALID = -100,  // (imaginary) flag that indicates this 
		//                        // ERevDataItems2 is not valid
	}

	public enum EDataUsage
	{
		NONE,
		KEY,
		INFO
	}

	public enum EDataType
	{
		BOOL,
		INT,
		STRING,
		ELEMENTID,
		VISIBILITY
	}

	public enum ERevSource
	{
		REV_SOURCE_DERIVED,
		REV_SOURCE_TAG,
		REV_SOURCE_CLOUD
	}
}