﻿namespace DiliBeneficiary.Core.Interfaces;

public interface IRepositoryManager
{
    IBeneficiaryRepository Beneficiary { get; }
    ISupportRepository Support { get; }
    IReferentRepository Referent { get; }
    ISchoolRegistrationRepository SchoolRegistration { get; }
    ITrainingTypeRepository TrainingType { get; }
    IProfessionRepository Profession { get; }
    IFormationRepository Formation { get; }
    IProfessionBilanRepository ProfessionBilan { get; }
    IBilanRepository Bilan { get; }
    ITrainingFieldRepository TrainingField { get; }
    IQuarterlyMonitoringRepository QuarterlyMonitoring { get; }
    IMonitoringActionRepository MonitoringAction { get; }
    void Save();
    void SaveAsync();
}