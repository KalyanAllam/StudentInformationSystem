
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SIS.Models
{

using System;
    using System.Collections.Generic;
    
public partial class Enrollment
{

    public int EnrollmentID { get; set; }

    public int CourseID { get; set; }

    public int StudentID { get; set; }

    public Nullable<int> TermID { get; set; }

    public Nullable<int> Marks { get; set; }

    public Nullable<int> MarksObtained { get; set; }



    public virtual Student Student { get; set; }

    public virtual Term Term { get; set; }

    public virtual Course Course { get; set; }

}

}
