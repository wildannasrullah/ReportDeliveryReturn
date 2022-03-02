using System;
using System.Data;
using System.Collections.Generic;
using SIMCore.Models;
using SIMCore.Services;
using SIMCore.Controllers;

namespace Expansion
{
    public class ExpansionHook
	{
        private DB db;
		private SQLService sql;
        private MasterReportViewerController controller;
		
		public ExpansionHook(DB db, SQLService sql, MasterReportViewerController controller)
		{
            this.db = db;
            this.sql = sql;
            this.controller = controller;
		}

        public ExpansionResult OnPost(string action, Dictionary<string, object> dictParameter, params object[] objectParameters)
		{
			try
			{
                /*
                    Console.WriteLine("message"); // Print something in SIMCore console
                    db.Dump(dictParameter); // Dump variable to SIMCore console
				    string role = db.GetCurrentRole(controller.HttpContext)); // Get current role
				    string user = db.GetCurrentUser(controller.HttpContext)); // Get current user
                */

			    //Do something here 	
				controller.tblSource = new DataTable();
				//controller.tblSource.Columns.Add("DocDate", typeof(DateTime));
				controller.tblSource.Columns.Add("DocNo", typeof(string));
				controller.tblSource.Columns.Add("Series", typeof(string));
				controller.tblSource.Columns.Add("DRDate", typeof(DateTime));
				controller.tblSource.Columns.Add("GIDocNo", typeof(string));
				controller.tblSource.Columns.Add("SODocNo", typeof(string));
				controller.tblSource.Columns.Add("CustomerCode", typeof(string));
				controller.tblSource.Columns.Add("CustomerName", typeof(string));
				controller.tblSource.Columns.Add("Location", typeof(string));
				controller.tblSource.Columns.Add("Zone", typeof(string));
				controller.tblSource.Columns.Add("Information", typeof(string));
				controller.tblSource.Columns.Add("Status", typeof(string));
				controller.tblSource.Columns.Add("PrintedBy", typeof(string));
				controller.tblSource.Columns.Add("PrintedDate", typeof(string));
				controller.tblSource.Columns.Add("CreatedBy", typeof(string));
				controller.tblSource.Columns.Add("CreatedDate", typeof(string));
				controller.tblSource.Columns.Add("ChangedBy", typeof(string));
				controller.tblSource.Columns.Add("ChangedDate", typeof(string));
				controller.tblSource.Columns.Add("MaterialCode", typeof(string));
				controller.tblSource.Columns.Add("MaterialName", typeof(string));
				controller.tblSource.Columns.Add("Info", typeof(string));
				controller.tblSource.Columns.Add("TagNo", typeof(string));
				controller.tblSource.Columns.Add("Unit", typeof(string));
				controller.tblSource.Columns.Add("QtyReturn", typeof(string));
				
			/*	
				string query = @"SELECT jrh.DocDate, jrh.Docno
					FROM jobresulth AS jrh
					WHERE jrh.Status<>'DELETED' " + dictParameter["additionalCondition"].ToString() + @"
					ORDER BY jrh.DocDate ASC";
			*/
				string query = @"SELECT h.DocDate, h.DocNo, h.Series, h.DocDate as DRDate, h.GIDocNo, g.SODocNo, h.CustomerCode, c.Name as CustomerName, h.Location, h.Zone, h.Information, h.Status,
									h.PrintedBy, h.PrintedDate, h.CreatedBy, h.CreatedDate, h.ChangedBy, h.ChangedDate, d.MaterialCode, m.Name as MaterialName, d.Info,
									d.TagNo, d.Unit, d.QtyReturn
									  FROM deliveryreturnh h
									  LEFT JOIN deliveryreturnd d ON h.DocNo=d.DocNo
									  LEFT JOIN mastercustomer c ON h.CustomerCode=c.Code
									  LEFT JOIN mastermaterial m ON d.MaterialCode=m.Code
									  LEFT JOIN goodsissueh g ON h.GIDocNo=g.DocNo
									where h.DocNo IS NOT NULL " + dictParameter["additionalCondition"].ToString() + @"
									order by h.DocNo DESC";
				
				
				DataTable tblHasil = sql.Select(query);
				
				// Fill tblSource
				DataRow newRow = null, lastRow = null;
				DateTime lastDocDate = DateTime.Parse("1/1/1900");
				string lastMachine = "", lastPrintCode = "";
				
				foreach(DataRow row in tblHasil.Rows)
				{
					if (lastDocDate.Date != ((DateTime)row["DocDate"]).Date)
					{
						newRow = controller.tblSource.NewRow();
						//newRow["DocDate"] 			= row["DocDate"];
						newRow["DocNo"] 			= row["DocNo"];
						newRow["Series"] 			= row["Series"];
						newRow["DRDate"]	   		= row["DRDate"];
						newRow["GIDocNo"] 			= row["GIDocNo"];
						newRow["SODocNo"] 			= row["SODocNo"];
						newRow["CustomerCode"] 		= row["CustomerCode"];
						newRow["CustomerName"] 		= row["CustomerName"];
						newRow["Location"] 			= row["Location"];
						newRow["Zone"] 				= row["Zone"];
						newRow["Information"] 		= row["Information"];
						newRow["Status"] 			= row["Status"];
						newRow["PrintedBy"] 		= row["PrintedBy"];
						newRow["PrintedDate"] 		= row["PrintedDate"];
						newRow["CreatedBy"] 		= row["CreatedBy"];
						newRow["CreatedDate"] 		= row["CreatedDate"];
						newRow["ChangedBy"] 		= row["ChangedBy"];
						newRow["ChangedDate"] 		= row["ChangedDate"];
						newRow["MaterialCode"] 		= row["MaterialCode"];
						newRow["MaterialName"] 		= row["MaterialName"];
						newRow["Info"] 				= row["Info"];
						newRow["TagNo"] 			= row["TagNo"];
						newRow["Unit"] 				= row["Unit"];
						newRow["QtyReturn"] 		= row["QtyReturn"];
						
						controller.tblSource.Rows.Add(newRow);
						
						if (lastDocDate.Date != ((DateTime)row["DocDate"]).Date)
						{
							lastRow = newRow;
						}
					}
					
					lastDocDate = ((DateTime)row["DocDate"]).Date;
					
				}
				controller.tblSource.AcceptChanges(); 
            } catch(Exception ex)
			{
				return new ExpansionResult(false, ex.Message);
            }
			
			return new ExpansionResult(true, "");
        }
	}
}