﻿using Application.PostFlower.DTOs;
using Application.UserApplication.DTOs;
using Domain.Constants.Enums;

namespace Application.Report.DTOs
{
    public class ReportModel
    {
        public Guid Id { get; set; }

        public int Rating { get; set; }

        public string Detail { get; set; }

        public ReportStatus Status { get; set; }

        public DateTime CreateAt { get; set; }

        public Guid CreateById { get; set; }

        public UserModel CreateBy { get; set; }

        public DateTime UpdateAt { get; set; }

        public Guid UpdateById { get; set; }

        public UserModel UpdateBy { get; set; }

        public Guid PostId { get; set; }

        public PostModel Post { get; set; }
    }
}
