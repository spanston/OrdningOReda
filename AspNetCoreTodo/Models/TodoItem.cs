using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCoreTodo.Models
{

    // This class defines what the database will need to store for each to-do item
    // An id, title, whether the item is complete and what the due date is. 
    //Represents a single item in the databse.
    public class TodoItem
    {

        public Guid Id { get; set; } //Globally unique identifier, long strings of letters and numbers
        public string UserId { get; set; }
        public bool IsDone { get; set; } //default is false
        [Required]//required attribute tells ASP.NET Core that this string can't be null or empty
        public string Title { get; set; }     
        public string ItemCategory { get; set; }        
        public Guid ItemListId { get; set; }

        //stores date/time stamp along with timezone offset from UTC, makes it easy to render dates accuretly on systems with df timezones
        //? marks the DueAt prop as nullable or optional, if ? wasn't included every todo item would need to have duedate. 
        public DateTimeOffset? DueAt { get; set; }
        public DateTimeOffset? DateRemoved { get; set; }


    }
}