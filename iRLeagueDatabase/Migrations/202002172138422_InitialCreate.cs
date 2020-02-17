namespace iRLeagueDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LeagueMemberEntities",
                c => new
                    {
                        MemberId = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        IRacingId = c.String(),
                        DanLisaId = c.String(),
                        DiscordId = c.String(),
                    })
                .PrimaryKey(t => t.MemberId);
            
            CreateTable(
                "dbo.SeasonEntities",
                c => new
                    {
                        SeasonId = c.Int(nullable: false, identity: true),
                        SeasonName = c.String(),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                        MainScoring_ScoringId = c.Int(),
                    })
                .PrimaryKey(t => t.SeasonId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.ScoringEntities", t => t.MainScoring_ScoringId)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId)
                .Index(t => t.MainScoring_ScoringId);
            
            CreateTable(
                "dbo.ScoringEntities",
                c => new
                    {
                        ScoringId = c.Int(nullable: false, identity: true),
                        DropWeeks = c.Int(nullable: false),
                        AverageRaceNr = c.Int(nullable: false),
                        ScheduleId = c.Int(),
                        ScoringRuleName = c.String(),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                    })
                .PrimaryKey(t => t.ScoringId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.ScheduleEntities", t => t.ScheduleId)
                .Index(t => t.ScheduleId)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId);
            
            CreateTable(
                "dbo.ScheduleEntities",
                c => new
                    {
                        ScheduleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                        Season_SeasonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ScheduleId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.SeasonEntities", t => t.Season_SeasonId, cascadeDelete: true)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId)
                .Index(t => t.Season_SeasonId);
            
            CreateTable(
                "dbo.SessionBaseEntities",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        SessionTitle = c.String(),
                        SessionType = c.Int(nullable: false),
                        Date = c.DateTime(),
                        LocationId = c.String(),
                        Duration = c.Time(nullable: false, precision: 7),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        RaceId = c.Int(),
                        Laps = c.Int(),
                        PracticeLength = c.Time(precision: 7),
                        QualyLength = c.Time(precision: 7),
                        RaceLength = c.Time(precision: 7),
                        IrSessionId = c.String(),
                        IrResultLink = c.String(),
                        QualyAttached = c.Boolean(),
                        PracticeAttached = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                        Schedule_ScheduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.ScheduleEntities", t => t.Schedule_ScheduleId, cascadeDelete: true)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId)
                .Index(t => t.Schedule_ScheduleId);
            
            CreateTable(
                "dbo.ResultEntities",
                c => new
                    {
                        ResultId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                    })
                .PrimaryKey(t => t.ResultId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.SessionBaseEntities", t => t.ResultId)
                .Index(t => t.ResultId)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId);
            
            CreateTable(
                "dbo.ResultRowEntities",
                c => new
                    {
                        ResultRowId = c.Int(nullable: false, identity: true),
                        ResultId = c.Int(nullable: false),
                        FinalPosition = c.Int(nullable: false),
                        StartPosition = c.Int(nullable: false),
                        FinishPosition = c.Int(nullable: false),
                        CarNumber = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        Car = c.String(),
                        CarClass = c.String(),
                        CompletedLaps = c.Int(nullable: false),
                        LeadLaps = c.Int(nullable: false),
                        FastLapNr = c.Int(nullable: false),
                        Incidents = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        RacePoints = c.Int(nullable: false),
                        BonusPoints = c.Int(nullable: false),
                        PenaltyPoints = c.Int(nullable: false),
                        QualifyingTime = c.Long(nullable: false),
                        Interval = c.Long(nullable: false),
                        AvgLapTime = c.Long(nullable: false),
                        FastestLapTime = c.Long(nullable: false),
                        PositionChange = c.Int(nullable: false),
                        IRacingId = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Member_MemberId = c.Int(),
                    })
                .PrimaryKey(t => new { t.ResultRowId, t.ResultId })
                .ForeignKey("dbo.LeagueMemberEntities", t => t.Member_MemberId)
                .ForeignKey("dbo.ResultEntities", t => t.ResultId, cascadeDelete: true)
                .Index(t => t.ResultId)
                .Index(t => t.Member_MemberId);
            
            CreateTable(
                "dbo.IncidentReviewEntities",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        OnLap = c.Int(nullable: false),
                        Corner = c.Int(nullable: false),
                        TimeStamp = c.Time(nullable: false, precision: 7),
                        VoteResult = c.Int(nullable: false),
                        VoteState = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        Author_MemberId = c.Int(),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                        MemberAtFault_MemberId = c.Int(),
                        Result_ResultId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.Author_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.MemberAtFault_MemberId)
                .ForeignKey("dbo.ResultEntities", t => t.Result_ResultId, cascadeDelete: true)
                .Index(t => t.Author_MemberId)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId)
                .Index(t => t.MemberAtFault_MemberId)
                .Index(t => t.Result_ResultId);
            
            CreateTable(
                "dbo.ReviewCommentEntities",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Vote = c.Int(nullable: false),
                        ReviewId = c.Int(),
                        Date = c.DateTime(),
                        Text = c.String(),
                        CreatedOn = c.DateTime(),
                        LastModifiedOn = c.DateTime(),
                        Version = c.Int(nullable: false),
                        Author_MemberId = c.Int(),
                        CreatedBy_MemberId = c.Int(),
                        LastModifiedBy_MemberId = c.Int(),
                        MemberAtFault_MemberId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.Author_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.CreatedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.LastModifiedBy_MemberId)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.MemberAtFault_MemberId)
                .ForeignKey("dbo.IncidentReviewEntities", t => t.ReviewId)
                .Index(t => t.ReviewId)
                .Index(t => t.Author_MemberId)
                .Index(t => t.CreatedBy_MemberId)
                .Index(t => t.LastModifiedBy_MemberId)
                .Index(t => t.MemberAtFault_MemberId);
            
            CreateTable(
                "dbo.IncidentReview_LeagueMember",
                c => new
                    {
                        ReviewRefId = c.Int(nullable: false),
                        MemberRefId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReviewRefId, t.MemberRefId })
                .ForeignKey("dbo.IncidentReviewEntities", t => t.ReviewRefId, cascadeDelete: true)
                .ForeignKey("dbo.LeagueMemberEntities", t => t.MemberRefId, cascadeDelete: true)
                .Index(t => t.ReviewRefId)
                .Index(t => t.MemberRefId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ScheduleEntities", "Season_SeasonId", "dbo.SeasonEntities");
            DropForeignKey("dbo.SeasonEntities", "MainScoring_ScoringId", "dbo.ScoringEntities");
            DropForeignKey("dbo.ScoringEntities", "ScheduleId", "dbo.ScheduleEntities");
            DropForeignKey("dbo.ResultEntities", "ResultId", "dbo.SessionBaseEntities");
            DropForeignKey("dbo.IncidentReviewEntities", "Result_ResultId", "dbo.ResultEntities");
            DropForeignKey("dbo.IncidentReviewEntities", "MemberAtFault_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.IncidentReviewEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.IncidentReview_LeagueMember", "MemberRefId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.IncidentReview_LeagueMember", "ReviewRefId", "dbo.IncidentReviewEntities");
            DropForeignKey("dbo.IncidentReviewEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ReviewCommentEntities", "ReviewId", "dbo.IncidentReviewEntities");
            DropForeignKey("dbo.ReviewCommentEntities", "MemberAtFault_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ReviewCommentEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ReviewCommentEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ReviewCommentEntities", "Author_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.IncidentReviewEntities", "Author_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ResultRowEntities", "ResultId", "dbo.ResultEntities");
            DropForeignKey("dbo.ResultRowEntities", "Member_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ResultEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ResultEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.SessionBaseEntities", "Schedule_ScheduleId", "dbo.ScheduleEntities");
            DropForeignKey("dbo.SessionBaseEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.SessionBaseEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ScheduleEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ScheduleEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ScoringEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.ScoringEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.SeasonEntities", "LastModifiedBy_MemberId", "dbo.LeagueMemberEntities");
            DropForeignKey("dbo.SeasonEntities", "CreatedBy_MemberId", "dbo.LeagueMemberEntities");
            DropIndex("dbo.IncidentReview_LeagueMember", new[] { "MemberRefId" });
            DropIndex("dbo.IncidentReview_LeagueMember", new[] { "ReviewRefId" });
            DropIndex("dbo.ReviewCommentEntities", new[] { "MemberAtFault_MemberId" });
            DropIndex("dbo.ReviewCommentEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.ReviewCommentEntities", new[] { "CreatedBy_MemberId" });
            DropIndex("dbo.ReviewCommentEntities", new[] { "Author_MemberId" });
            DropIndex("dbo.ReviewCommentEntities", new[] { "ReviewId" });
            DropIndex("dbo.IncidentReviewEntities", new[] { "Result_ResultId" });
            DropIndex("dbo.IncidentReviewEntities", new[] { "MemberAtFault_MemberId" });
            DropIndex("dbo.IncidentReviewEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.IncidentReviewEntities", new[] { "CreatedBy_MemberId" });
            DropIndex("dbo.IncidentReviewEntities", new[] { "Author_MemberId" });
            DropIndex("dbo.ResultRowEntities", new[] { "Member_MemberId" });
            DropIndex("dbo.ResultRowEntities", new[] { "ResultId" });
            DropIndex("dbo.ResultEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.ResultEntities", new[] { "CreatedBy_MemberId" });
            DropIndex("dbo.ResultEntities", new[] { "ResultId" });
            DropIndex("dbo.SessionBaseEntities", new[] { "Schedule_ScheduleId" });
            DropIndex("dbo.SessionBaseEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.SessionBaseEntities", new[] { "CreatedBy_MemberId" });
            DropIndex("dbo.ScheduleEntities", new[] { "Season_SeasonId" });
            DropIndex("dbo.ScheduleEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.ScheduleEntities", new[] { "CreatedBy_MemberId" });
            DropIndex("dbo.ScoringEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.ScoringEntities", new[] { "CreatedBy_MemberId" });
            DropIndex("dbo.ScoringEntities", new[] { "ScheduleId" });
            DropIndex("dbo.SeasonEntities", new[] { "MainScoring_ScoringId" });
            DropIndex("dbo.SeasonEntities", new[] { "LastModifiedBy_MemberId" });
            DropIndex("dbo.SeasonEntities", new[] { "CreatedBy_MemberId" });
            DropTable("dbo.IncidentReview_LeagueMember");
            DropTable("dbo.ReviewCommentEntities");
            DropTable("dbo.IncidentReviewEntities");
            DropTable("dbo.ResultRowEntities");
            DropTable("dbo.ResultEntities");
            DropTable("dbo.SessionBaseEntities");
            DropTable("dbo.ScheduleEntities");
            DropTable("dbo.ScoringEntities");
            DropTable("dbo.SeasonEntities");
            DropTable("dbo.LeagueMemberEntities");
        }
    }
}
