using FaisalLeague.Data.Access.Maps.Common;
using FaisalLeague.Data.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Data.Access.Maps.Main
{
    public class VariousMap : IMap
    {
        public void Visit(ModelBuilder builder)
        {
            builder.Entity<Question>()
                .ToTable("Questions")
                .HasKey(x => x.Id);

            builder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(x => x.Id);

            builder.Entity<Card>()
                .ToTable("Cards")
                .HasKey(x => x.Id);

            builder.Entity<CardState>()
                .ToTable("CardStates")
                .HasKey(x => x.Id);

            builder.Entity<QuestionPoint>()
                .ToTable("QuestionPoints")
                .HasKey(x => x.Id);

            builder.Entity<Choice>()
                .ToTable("Choices")
                .HasKey(x => x.Id);

            builder.Entity<UserCard>()
                .ToTable("UserCards")
                .HasKey(x => x.Id);

            builder.Entity<CardQuestion>()
                .ToTable("CardQuestions")
                .HasKey(x => new { x.CardId, x.QuestionId, });

            builder.Entity<UserAnswer>()
                .ToTable("UserAnswers")
                .HasKey(x => new { x.UserCardId, x.QuestionId });
        }
    }
}
