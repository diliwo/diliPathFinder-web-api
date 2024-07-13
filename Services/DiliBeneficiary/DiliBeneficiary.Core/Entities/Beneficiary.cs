﻿using System.Text.RegularExpressions;
using DiliBeneficiary.Core.Common;
using DiliBeneficiary.Core.Enums;
using DiliBeneficiary.Core.Exceptions;
using DiliBeneficiary.Core.ValueObjects;

namespace DiliBeneficiary.Core.Entities
{
    public class Beneficiary : AggregateRoot
    {
        public int? SourceId { get; set; }
        public string ReferenceNumber { get; set; }
        public CivilStatus CivilStatus { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
        public Gender Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime StartDateInCpas { get; set; }
        public DateTime? EndDateInCpas { get; set; }
        public string Nationality { get; set; }
        public string Niss { get; set; }
        public Email Email { get; set; }
        public Phone Phone { get; set; }
        public Phone MobilePhone { get; set; }
        public bool HasChildren { get; set; }
        public ValueObjects.Language NativeLanguage { get; set; }
        public ValueObjects.Language ContactLanguage { get; set; }
        public Address Address { get; set; }
        public string SocialWorkerName { get; set; }
        public DateTime? LastApppointmentDate { get; set; }
        public DateTime? LastEvaluationDate { get; set; }
        public ContratPiis? Piis { get; set; }
        public string IbisNumber { get; set; }
        public virtual IList<Support> Supports { get; set; } = new List<Support>();
        public virtual IList<Candidacy> Candidacies { get; set; } = new List<Candidacy>();
        public virtual IList<SchoolRegistration> SchoolRegistrations { get; set; } = new List<SchoolRegistration>();
        public virtual IList<ProfessionnalExperience> ProfessionnalExpectations { get; set; } = new List<ProfessionnalExperience>();
        public virtual IList<Bilan> Bilans { get; set; } = new List<Bilan>();
        public virtual IList<QuarterlyMonitoring> QuarterlyMonitorings { get; set; } = new List<QuarterlyMonitoring>();
        public IEnumerable<SchoolRegistration> Registrations
        {
            get => SchoolRegistrations;
        }

        public IEnumerable<Candidacy> Candidates
        {
            get => Candidacies;
        }

        public Beneficiary()
        {
        }

        public Beneficiary(
            string referenceNumber, 
            CivilStatus civilStatus, 
            string firstFirstName,
            string lastName,
            Gender gender,
            DateTime? birthDate,
            string nationality,
            string niss,
            Email email,
            Phone phone,
            Phone mobilePhone,
            ValueObjects.Language nativeLanguage,
            ValueObjects.Language contactLanguage,
            Address address,
            string socialWorkerName,
            bool hasChildren = false) {

            if (string.IsNullOrEmpty(firstFirstName))
            {
                throw new ArgumentNullException(nameof(firstFirstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            if (string.IsNullOrEmpty(referenceNumber))
            {
                throw new ArgumentNullException(nameof(referenceNumber));
            }

            string nissPattern = @"^[0-9]{8,11}$";

            if (!Regex.Match(niss, nissPattern, RegexOptions.IgnoreCase).Success)
            {
                throw new InvalidNissFormatException(niss);
            }

            string referenceNumberPattern = @"^[0-9]{1,6}-[0-9]{2}$";
            if (!Regex.Match(referenceNumber, referenceNumberPattern, RegexOptions.IgnoreCase).Success)
            {
                throw new InvalidReferenceNumberFormatException(referenceNumber);
            }

            ReferenceNumber = referenceNumber;
            CivilStatus = civilStatus;
            FirstName = firstFirstName;
            LastName = lastName;
            Gender = gender;
            BirthDate = birthDate;
            Nationality = nationality;
            Niss = niss;
            Email = email;
            Phone = phone;
            MobilePhone = mobilePhone;
            NativeLanguage = nativeLanguage;
            ContactLanguage = contactLanguage;
            Address = address;
            SocialWorkerName = socialWorkerName;
            HasChildren = hasChildren;
        }

        public void UpdateNativeLanguage(string nativeLanguage)
        {
            string nativeLanguagePattern = @"^[a-zA-Z àâäèéêëîïôœùûüÿçÀÂÄÈÉÊËÎÏÔŒÙÛÜŸÇ]{4,}$";
            if (!string.IsNullOrWhiteSpace(nativeLanguage) && !Regex.Match(nativeLanguage, nativeLanguagePattern, RegexOptions.IgnoreCase).Success)
            {
                throw new InvalidLanguageNameFormatException(nativeLanguage);
            }

            NativeLanguage.SpokenLanguage = nativeLanguage;
        }

        public void AddPiis(ContratPiis piis)
        {
            if (Piis is null)
            {
                Piis = piis;
            }
            else if(Piis is not null && piis is not null)
            {
                Piis.StartDate = piis.StartDate;
                Piis.EndDate = piis.EndDate;
                Piis.CommentaireStatut = piis.CommentaireStatut;
                Piis.Libelle = piis.Libelle;
                Piis.AsTraitant = piis.AsTraitant;
                Piis.DateOfSigning = piis.DateOfSigning;
                Piis.IdSource = piis.IdSource;
            }
        }

        public bool IsCurrentlyHired
        {
            get
            {
                var today = DateTime.Today;
                if (Candidacies.Any(c => c.IsHired 
                                         && ((c.JobOffer.StartOccupationDate <= today && c.JobOffer.EndOccupationDate >= today) 
                                             || c.JobOffer.StartOccupationDate <= today && c.JobOffer.EndOccupationDate == null)))
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsAlreadyHiredDuringThisPeriod(DateTime date)
        {
            if (Candidacies.Any(c => c.Softdelete != true && c.IsHired
                                                          && ((c.JobOffer.StartOccupationDate <= date && c.JobOffer.EndOccupationDate >= date)
                                                              || c.JobOffer.StartOccupationDate <= date && c.JobOffer.EndOccupationDate == null)))
            {
                return true;
            }
            return false;
        }
        public bool IsAlreadyHiredDuringThisPeriod(DateTime date, int jobOfferId)
        {
            var application = Candidacies.SingleOrDefault(c => c.Softdelete != true && c.IsHired
                && ((c.JobOffer.StartOccupationDate <= date && c.JobOffer.EndOccupationDate >= date)
                    || c.JobOffer.StartOccupationDate <= date && c.JobOffer.EndOccupationDate == null));

            if (application is not null && application.JobOfferId != jobOfferId)
            {
                return true;
            }

            return false;
        }

        public string UpsertIbis(string ibis)
        {
            if (!string.IsNullOrWhiteSpace(ibis))
            {
                IbisNumber = ibis;
            }

            return IbisNumber;
        }
    }
}