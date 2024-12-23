using Manage.Models.NetWorth.Base;
using Manage.Models.NetWorth.DTO;
using Manage.Models.NetWorth;
using Manage.Models.NetWorth.Enum;

namespace Manage.Models.NetWorth.Factory
{
    // Metodo che converte il DTO generico in un'istanza della classe appropriata.
    public class InvestimentoFactory
    {
        public static InvestimentoBase CreateInvestimento(InvestimentoDtoBase dto)
        {
            switch (dto.TipoInvestimento)
            {
                // Azione: 1
                case TipoInvestimentoEnum.Azioni:
                    var azioneDTO = (AzioneDTO)dto;
                    return new Azione
                    {
                        // Dati classe Base
                        Nome = azioneDTO.Nome,
                        Ticker = azioneDTO.Ticker,
                        Isin = azioneDTO.Isin,
                        PrezzoAttualeInvestimento = azioneDTO.PrezzoAttualeInvestimento,
                        PrezzoMedio = azioneDTO.PrezzoMedio,
                        PrezzoMinimo = azioneDTO.PrezzoMinimo,
                        PrezzoMassimo = azioneDTO.PrezzoMassimo,
                        TipoInvestimento = azioneDTO.TipoInvestimento,

                        // Dati classe Azione
                        Settore = azioneDTO.Settore,
                        IsAccumulo = azioneDTO.IsAccumulo,
                        DividendYield = azioneDTO.DividendYield
                    };

                // Obbligazione: 2
                case TipoInvestimentoEnum.Obbligazioni:
                    var obbligazioneDTO = (ObbligazioneDTO)dto;
                    return new Obbligazione
                    {
                        // Dati classe Base
                        Nome = obbligazioneDTO.Nome,
                        Ticker = obbligazioneDTO.Ticker,
                        Isin = obbligazioneDTO.Isin,
                        PrezzoAttualeInvestimento = obbligazioneDTO.PrezzoAttualeInvestimento,
                        PrezzoMedio = obbligazioneDTO.PrezzoMedio,
                        PrezzoMinimo = obbligazioneDTO.PrezzoMinimo,
                        PrezzoMassimo = obbligazioneDTO.PrezzoMassimo,
                        TipoInvestimento = obbligazioneDTO.TipoInvestimento,

                        // Dati classe Obbligazione
                        CedolaAnnua = obbligazioneDTO.CedolaAnnua,
                        DataScadenza = obbligazioneDTO.DataScadenza,
                        HasPenalitaAnticipata = obbligazioneDTO.HasPenalitaAnticipata,
                        PenalitaAnticipataPercentuale = obbligazioneDTO.PenalitaAnticipataPercentuale
                    };

                // Cryptovaluta: 3
                case TipoInvestimentoEnum.Cryptovalute:
                    var cryptovaluteDTO = (CryptovalutaDTO)dto;
                    return new Cryptovaluta
                    {
                        // Dati classe Base
                        Nome = cryptovaluteDTO.Nome,
                        Ticker = cryptovaluteDTO.Ticker,
                        Isin = cryptovaluteDTO.Isin,
                        PrezzoAttualeInvestimento = cryptovaluteDTO.PrezzoAttualeInvestimento,
                        PrezzoMedio = cryptovaluteDTO.PrezzoMedio,
                        PrezzoMinimo = cryptovaluteDTO.PrezzoMinimo,
                        PrezzoMassimo = cryptovaluteDTO.PrezzoMassimo,
                        TipoInvestimento = cryptovaluteDTO.TipoInvestimento,

                        // Dati classe Cryptovaluta
                        Blockchain = cryptovaluteDTO.Blockchain,
                        TassoStaking = cryptovaluteDTO.TassoStaking
                    };

                // Titolo di stato: 4
                case TipoInvestimentoEnum.TitoliDiStato:
                    var titoliDto = (TitoloDiStatoDTO)dto;
                    return new TitoloDiStato
                    {
                        // Dati classe Base
                        Nome = titoliDto.Nome,
                        Ticker = titoliDto.Ticker,
                        Isin = titoliDto.Isin,
                        PrezzoAttualeInvestimento = titoliDto.PrezzoAttualeInvestimento,
                        PrezzoMedio = titoliDto.PrezzoMedio,
                        PrezzoMinimo = titoliDto.PrezzoMinimo,
                        PrezzoMassimo = titoliDto.PrezzoMassimo,
                        TipoInvestimento = titoliDto.TipoInvestimento,

                        // Dati classe TitoloDiStato
                        Cedole = titoliDto.Cedole,
                        BonusMantenimento = titoliDto.BonusMantenimento,
                        PenalitaAnticipata = titoliDto.PenalitaAnticipata,
                        ValoreRimborso = titoliDto.ValoreRimborso,
                        PenalitaPercentuale = titoliDto.PenalitaPercentuale,
                        HasPenalita = titoliDto.HasPenalita,
                    };

                // Conto deposito: 5
                case TipoInvestimentoEnum.ContoDeposito:
                    var contoDepositoDto = (ContoDepositoDTO)dto;
                    return new ContoDeposito
                    {
                        // Dati classe Base
                        Nome = contoDepositoDto.Nome,
                        Ticker = contoDepositoDto.Ticker,
                        Isin = contoDepositoDto.Isin,
                        PrezzoAttualeInvestimento = contoDepositoDto.PrezzoAttualeInvestimento,
                        PrezzoMedio = contoDepositoDto.PrezzoMedio,
                        PrezzoMinimo = contoDepositoDto.PrezzoMinimo,
                        PrezzoMassimo = contoDepositoDto.PrezzoMassimo,
                        TipoInvestimento = contoDepositoDto.TipoInvestimento,

                        // Dati classe ContoDeposito
                        Tassi = contoDepositoDto.Tassi,
                        HasPenalita = contoDepositoDto.HasPenalita,
                        PenalitaPercentuale = contoDepositoDto.PenalitaPercentuale,
                        AliquotaTasse = contoDepositoDto.AliquotaTasse,
                        ImpostaBolloAnnuale = contoDepositoDto.ImpostaBolloAnnuale,
                        CostoGestioneFisso = contoDepositoDto.CostoGestioneFisso,
                    };

                // Fondo pensione: 6
                case TipoInvestimentoEnum.FondoPensione:
                    var fondoPensioneDto = (FondoPensioneDTO)dto;
                    return new FondoPensione
                    {
                        // Dati classe Base
                        Nome = fondoPensioneDto.Nome,
                        Ticker = fondoPensioneDto.Ticker,
                        Isin = fondoPensioneDto.Isin,
                        PrezzoAttualeInvestimento = fondoPensioneDto.PrezzoAttualeInvestimento,
                        PrezzoMedio = fondoPensioneDto.PrezzoMedio,
                        PrezzoMinimo = fondoPensioneDto.PrezzoMinimo,
                        PrezzoMassimo = fondoPensioneDto.PrezzoMassimo,
                        TipoInvestimento = fondoPensioneDto.TipoInvestimento,

                        // Dati classe FondoPensione
                        PercentualeStipendio = fondoPensioneDto.PercentualeStipendio,
                        PercentualeStipendioDatoreLavoro = fondoPensioneDto.PercentualeStipendioDatoreLavoro,
                        InteresseAnnuale = fondoPensioneDto.InteresseAnnuale,
                        PenalitaPrelievoAnticipatoAttiva = fondoPensioneDto.PenalitaPrelievoAnticipatoAttiva,
                        PenalitaPrelievoPercentuale = fondoPensioneDto.PenalitaPrelievoPercentuale
                    };

                // Buoni fruttiferi postali: 7
                case TipoInvestimentoEnum.BuoniFruttiferiPostali:
                    var buoniFruttiferiPostaliDto = (BuoniFruttiferiPostaliDTO)dto;
                    return new BuoniFruttiferiPostali
                    {
                        // Dati classe Base
                        Nome = buoniFruttiferiPostaliDto.Nome,
                        Ticker = buoniFruttiferiPostaliDto.Ticker,
                        Isin = buoniFruttiferiPostaliDto.Isin,
                        PrezzoAttualeInvestimento = buoniFruttiferiPostaliDto.PrezzoAttualeInvestimento,
                        PrezzoMedio = buoniFruttiferiPostaliDto.PrezzoMedio,
                        PrezzoMinimo = buoniFruttiferiPostaliDto.PrezzoMinimo,
                        PrezzoMassimo = buoniFruttiferiPostaliDto.PrezzoMassimo,
                        TipoInvestimento = buoniFruttiferiPostaliDto.TipoInvestimento,

                        // Dati classe BuoniFruttiferiPostali
                        Rendimenti = buoniFruttiferiPostaliDto.Rendimenti,
                        PenalitaPrelievoAnticipatoAttiva = buoniFruttiferiPostaliDto.PenalitaPrelievoAnticipatoAttiva,
                        PenalitaPercentuale = buoniFruttiferiPostaliDto.PenalitaPercentuale
                    };

                // Immobili: 8
                case TipoInvestimentoEnum.Immobili:
                    var immobileDto = (ImmobileDTO)dto;
                    return new Immobile
                    {
                        // Dati classe Base
                        Nome = immobileDto.Nome,
                        Ticker = immobileDto.Ticker,
                        Isin = immobileDto.Isin,
                        PrezzoAttualeInvestimento = immobileDto.PrezzoAttualeInvestimento,
                        PrezzoMedio = immobileDto.PrezzoMedio,
                        PrezzoMinimo = immobileDto.PrezzoMinimo,
                        PrezzoMassimo = immobileDto.PrezzoMassimo,
                        TipoInvestimento = immobileDto.TipoInvestimento,

                        // Dati classe Immobili
                        PrezzoAcquisto = immobileDto.PrezzoAcquisto,
                        ValoreAttuale = immobileDto.ValoreAttuale,
                        RedditoAnnualeAffitto = immobileDto.RedditoAnnualeAffitto,                          
                        CostiGestioneAnnui = immobileDto.CostiGestioneAnnui,
                        AliquotaFiscale = immobileDto.AliquotaFiscale,
                        DataAcquisto = immobileDto.DataAcquisto
                    };

                // Altri casi per altri tipi di investimento
                default:
                    throw new InvalidOperationException("Tipo di investimento non supportato");
            }
        }

    }
}
