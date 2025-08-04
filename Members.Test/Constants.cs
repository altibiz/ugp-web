namespace Members.Test
{
    class Constants
    {
        public const string jsonbnkstatement= @"
{
	""Data"": [
		{
			""BankID"": 29,
			""Bank"": ""BANKNAME"",
			""StatementID"": 1,
			""StatementNumber"": 1,
			""Number"": ""2020-00000000-0000000000"",
			""PaymentDescription"": ""PAYMENT"",
			""Date"": ""2020-04-16T00:00:00"",
			""PR"": ""P"",
			""Type"": ""Uplata"",
			""Payable"": 0.0,
			""Receivable"": 500.0,
			""IsProcessed"": false,
			""IsPosted"": false,
			""COA"": null,
			""CostCenterID"": null,
			""AnalyticsID"": null,
			""ClosedYear"": 0,
			""ClosedNumber"": 0,
			""IsClosed"": false,
			""Audit"": null,
			""Year"": 2020,
			""ID"": 6,
			""AccountID"": 1,
			""Account"": null,
			""PRN"": {
				""Model"": null,
				""Number"": ""HR99 99""
			},
			""RRN"": {
				""Model"": null,
				""Number"": ""HR99 333""
			},
			""Amount"": 1100.0,
			""Description"": null,
			""ISO20022"": null,
			""CurrencyID"": 893,
			""Currency"": {
				""ID"": 893,
				""Code"": ""HRK"",
				""Name"": ""Kuna"",
				""Sign"": ""kn"",
				""Lookup"": ""893 - HRK""
			},
			""Partner"": {
				""Name"": ""Something someone"",
				""Address"": {
					""Street"": null,
					""Postcode"": null,
					""City"": ""Test City"",
					""CountryID"": 385,
					""Country"": null
				},
				""Account"": ""HR44444""
			}
		},
		{
			""BankID"": 29,
			""Bank"": ""BANKNAME"",
			""StatementID"": 1,
			""StatementNumber"": 1,
			""Number"": ""2020-00000000-0000000000"",
			""PaymentDescription"": ""PAYMENT"",
			""Date"": ""2020-04-16T00:00:00"",
			""PR"": ""P"",
			""Type"": ""Uplata"",
			""Payable"": 0.0,
			""Receivable"": 100.0,
			""IsProcessed"": false,
			""IsPosted"": false,
			""COA"": null,
			""CostCenterID"": null,
			""AnalyticsID"": null,
			""ClosedYear"": 0,
			""ClosedNumber"": 0,
			""IsClosed"": false,
			""Audit"": null,
			""Year"": 2020,
			""ID"": 2,
			""AccountID"": 1,
			""Account"": null,
			""PRN"": {
				""Model"": null,
				""Number"": ""HR99 99""
			},
			""RRN"": {
				""Model"": null,
				""Number"": ""HR99 99""
			},
			""Amount"": 100.0,
			""Description"": null,
			""ISO20022"": null,
			""CurrencyID"": 893,
			""Currency"": {
				""ID"": 893,
				""Code"": ""HRK"",
				""Name"": ""Kuna"",
				""Sign"": ""kn"",
				""Lookup"": ""893 - HRK""
			},
			""Partner"": {
				""Name"": ""Someone else"",
				""Address"": {
					""Street"": null,
					""Postcode"": null,
					""City"": ""Another City"",
					""CountryID"": 385,
					""Country"": null
				},
				""Account"": ""HR111111""
			}
		}
	],
	""Count"": 2
}";

		public const string xmlbnkstatmnt = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:camt.053.001.08"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:fn=""http://www.w3.org/2005/xpath-functions"">
  <BkToCstmrStmt>
    <GrpHdr>
      <MsgId>053.0000000000000000000001</MsgId>
      <CreDtTm>2025-01-01T00:00:00.000</CreDtTm>
    </GrpHdr>
    <Stmt>
      <Id>CAMT053/0000000000/000/2025/00/001</Id>
      <LglSeqNb>00</LglSeqNb>
      <CreDtTm>2025-01-01T00:00:00.000</CreDtTm>
      <FrToDt>
        <FrDtTm>2025-01-01T00:00:00.000</FrDtTm>
        <ToDtTm>2025-01-01T23:59:59.999</ToDtTm>
      </FrToDt>
      <RptgSrc>
        <Prtry>DUMMY_SOURCE</Prtry>
      </RptgSrc>
      <Acct>
        <Id>
          <IBAN>XX0000000000000000000</IBAN>
        </Id>
        <Ccy>CUR</Ccy>
        <Nm>DUMMY ACCOUNT</Nm>
        <Svcr>
          <FinInstnId>
            <BICFI>DUMMYBIC</BICFI>
          </FinInstnId>
        </Svcr>
      </Acct>
      <Bal>
        <Tp>
          <CdOrPrtry>
            <Cd>OPBD</Cd>
          </CdOrPrtry>
        </Tp>
        <Amt Ccy=""CUR"">1000.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <Dt>
          <Dt>2025-01-01</Dt>
        </Dt>
      </Bal>
      <Bal>
        <Tp>
          <CdOrPrtry>
            <Cd>CLBD</Cd>
          </CdOrPrtry>
        </Tp>
        <Amt Ccy=""CUR"">900.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <Dt>
          <Dt>2025-01-01</Dt>
        </Dt>
      </Bal>
      <TxsSummry>
        <TtlCdtNtries>
          <NbOfNtries>2</NbOfNtries>
          <Sum>600.00</Sum>
        </TtlCdtNtries>
        <TtlDbtNtries>
          <NbOfNtries>1</NbOfNtries>
          <Sum>100.00</Sum>
        </TtlDbtNtries>
      </TxsSummry>
      <Ntry>
        <NtryRef>00000000000001</NtryRef>
        <Amt Ccy=""CUR"">100.00</Amt>
        <CdtDbtInd>DBIT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-01-01</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-01-01</Dt>
        </ValDt>
        <AcctSvcrRef>00000000000000000001</AcctSvcrRef>
        <BkTxCd>
          <Domn>
            <Cd>PMNT</Cd>
            <Fmly>
              <Cd>ICDT</Cd>
              <SubFmlyCd>NTAV</SubFmlyCd>
            </Fmly>
          </Domn>
          <Prtry>
            <Cd>NOTPROVIDED</Cd>
          </Prtry>
        </BkTxCd>
        <NtryDtls>
          <TxDtls>
            <Refs>
              <AcctSvcrRef>00000000000000000001</AcctSvcrRef>
              <EndToEndId>XX00</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""CUR"">100.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Cdtr>
                <Pty>
                  <Nm>DUMMY CREDITOR</Nm>
                  <PstlAdr>
                    <AdrLine>123 Dummy St</AdrLine>
                    <AdrLine>Dummyville</AdrLine>
                  </PstlAdr>
                </Pty>
              </Cdtr>
              <CdtrAcct>
                <Id>
                  <IBAN>XX0000000000000000001</IBAN>
                </Id>
              </CdtrAcct>
            </RltdPties>
            <Purp>
              <Cd>DUMMY</Cd>
            </Purp>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>XX0000000000001</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy payment</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000001</NtryRef>
        <Amt Ccy=""CUR"">30.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-01-01</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-01-01</Dt>
        </ValDt>
        <AcctSvcrRef>00000000000000000001</AcctSvcrRef>
        <BkTxCd>
          <Domn>
            <Cd>PMNT</Cd>
            <Fmly>
              <Cd>RCDT</Cd>
              <SubFmlyCd>NTAV</SubFmlyCd>
            </Fmly>
          </Domn>
          <Prtry>
            <Cd>NOTPROVIDED</Cd>
          </Prtry>
        </BkTxCd>
        <NtryDtls>
          <TxDtls>
            <Refs>
              <AcctSvcrRef>00000000000000000001</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0001</InstrId>
              <EndToEndId>XX00</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""CUR"">30.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DUMMY COMPANY</Nm>
                  <PstlAdr>
                    <AdrLine>123 Dummy St</AdrLine>
                    <AdrLine>Dummyville</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000001</IBAN>
                </Id>
              </DbtrAcct>
              <UltmtDbtr>
                <Pty>
                  <Nm>DUMMY ULTIMATE DEBTOR</Nm>
                  <Id>
                    <PrvtId>
                      <Othr>
                        <Id>DUMMYID</Id>
                      </Othr>
                    </PrvtId>
                  </Id>
                </Pty>
              </UltmtDbtr>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>XX0000000000001</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy statement - 30</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      </Stmt>
  </BkToCstmrStmt>
</Document>
";
	}
}
