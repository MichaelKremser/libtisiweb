using System;
using System.Collections.Generic;

/*
 * *ti*ny *si*mple web management system
 * (C) Michael Kremser, 2003-2015
 * 
 * This is free software.
 * License: MIT
*/

namespace mkcs.libtisiweb.Models
{
	public class Category
	{
		public string ID { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public List<Category> Categories { get; set; }

		public Category()
		{
			Categories = new List<Category>();
		}

		public override string ToString ()
		{
			return string.Format("[Category: ID={0}, Description={1}, IsActive={2}, Categories={3}]", ID, Description, IsActive, Categories.Count);
		}
	}
}
