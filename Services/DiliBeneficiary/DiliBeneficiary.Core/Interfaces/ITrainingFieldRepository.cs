﻿using DiliBeneficiary.Core.Entities;

namespace DiliBeneficiary.Core.Interfaces
{
    public interface ITrainingFieldRepository
    {
        void Persist(TrainingField field);
        TrainingField GetById(int fieldId);
        IQueryable<TrainingField> GetFields(string filter = "", string orderBy = "");
        void SoftDelete(TrainingField field);
    }
}