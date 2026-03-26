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


        public const string xmlBankMultipleStatetments = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:camt.053.001.08"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:fn=""http://www.w3.org/2005/xpath-functions"">
  <BkToCstmrStmt>
    <GrpHdr>
      <MsgId>053.0000000000000000000002</MsgId>
      <CreDtTm>2025-12-29T01:53:07.174</CreDtTm>
    </GrpHdr>
    <Stmt>
      <Id>CAMT053/0000000000/000/2025/01/001</Id>
      <LglSeqNb>156</LglSeqNb>
      <CreDtTm>2025-12-29T01:53:07.174</CreDtTm>
      <FrToDt>
        <FrDtTm>2025-12-22T00:00:00.000</FrDtTm>
        <ToDtTm>2025-12-24T23:59:59.999</ToDtTm>
      </FrToDt>
      <RptgSrc>
        <Prtry>DUMMY_SOURCE</Prtry>
      </RptgSrc>
      <Acct>
        <Id>
          <IBAN>XX0000000000000000000</IBAN>
        </Id>
        <Ccy>EUR</Ccy>
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
        <Amt Ccy=""EUR"">5184.93</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <Dt>
          <Dt>2025-12-22</Dt>
        </Dt>
      </Bal>
      <Bal>
        <Tp>
          <CdOrPrtry>
            <Cd>CLBD</Cd>
          </CdOrPrtry>
        </Tp>
        <Amt Ccy=""EUR"">5319.93</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <Dt>
          <Dt>2025-12-22</Dt>
        </Dt>
      </Bal>
      <TxsSummry>
        <TtlCdtNtries>
          <NbOfNtries>5</NbOfNtries>
          <Sum>135.00</Sum>
        </TtlCdtNtries>
        <TtlDbtNtries>
          <NbOfNtries>0</NbOfNtries>
          <Sum>0.00</Sum>
        </TtlDbtNtries>
      </TxsSummry>
      <Ntry>
        <NtryRef>00000000000001</NtryRef>
        <Amt Ccy=""EUR"">30.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-22</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-22</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000001-00000000001</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000001-00000000001</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0001</InstrId>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">30.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DUMMY COMPANY A D.O.O.</Nm>
                  <PstlAdr>
                    <StrtNm>DUMMY STREET</StrtNm>
                    <BldgNb>1</BldgNb>
                    <PstCd>00001</PstCd>
                    <TwnNm>DUMMYTOWN</TwnNm>
                    <CtrySubDvsn>Dummy County</CtrySubDvsn>
                    <Ctry>HR</Ctry>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000001</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR0000-00000000001</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy donation - annual partner</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000002</NtryRef>
        <Amt Ccy=""EUR"">30.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-22</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-22</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000002-00000000002</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000002-00000000002</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0002</InstrId>
              <EndToEndId>XX000000000000-0002</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">30.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>JANE DOE</Nm>
                  <PstlAdr>
                    <AdrLine>123 Dummy Ave</AdrLine>
                    <AdrLine>Dummyville</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000002</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <Purp>
              <Cd>COST</Cd>
            </Purp>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR0000-00000000002</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy annual fee</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000003</NtryRef>
        <Amt Ccy=""EUR"">30.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-22</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-22</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000003-00000000003</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000003-00000000003</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0003</InstrId>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">30.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DUMMY COMPANY B D.O.O.</Nm>
                  <PstlAdr>
                    <Ctry>HR</Ctry>
                    <AdrLine>DUMMY ROAD 36</AdrLine>
                    <AdrLine>00000 DUMMYCITY</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000003</IBAN>
                </Id>
              </DbtrAcct>
              <UltmtDbtr>
                <Pty>
                  <Nm>Dummy company B doo</Nm>
                </Pty>
              </UltmtDbtr>
            </RltdPties>
            <Purp>
              <Cd>COST</Cd>
            </Purp>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR0000-00000000003</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy annual fee - 30</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000004</NtryRef>
        <Amt Ccy=""EUR"">15.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-22</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-22</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000004-00000000004</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000004-00000000004</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0004</InstrId>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">15.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DOE JOHN</Nm>
                  <PstlAdr>
                    <Ctry>HR</Ctry>
                    <AdrLine>DUMMY LANE 17</AdrLine>
                    <AdrLine>00000 DUMMYCITY</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000004</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR99</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy one-time donation</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000005</NtryRef>
        <Amt Ccy=""EUR"">30.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-22</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-22</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000005-00000000005</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000005-00000000005</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0005</InstrId>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">30.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>ALICE BOB SMITH</Nm>
                  <PstlAdr>
                    <AdrLine>DUMMYTOWN DUMMY STREET 6</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000005</IBAN>
                </Id>
              </DbtrAcct>
              <UltmtDbtr>
                <Pty>
                  <Nm>Dummy commerce d.o.o. Dummy Pharmacy</Nm>
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
                  <Ref>HR0000000005</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy one-time donation on behalf of Dummy Pharmacy</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
    </Stmt>
    <Stmt>
      <Id>CAMT053/0000000000/000/2025/02/001</Id>
      <LglSeqNb>157</LglSeqNb>
      <CreDtTm>2025-12-29T01:53:07.174</CreDtTm>
      <FrToDt>
        <FrDtTm>2025-12-24T00:00:00.000</FrDtTm>
        <ToDtTm>2025-12-24T23:59:59.999</ToDtTm>
      </FrToDt>
      <RptgSrc>
        <Prtry>DUMMY_SOURCE</Prtry>
      </RptgSrc>
      <Acct>
        <Id>
          <IBAN>XX0000000000000000000</IBAN>
        </Id>
        <Ccy>EUR</Ccy>
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
        <Amt Ccy=""EUR"">5319.93</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <Dt>
          <Dt>2025-12-24</Dt>
        </Dt>
      </Bal>
      <Bal>
        <Tp>
          <CdOrPrtry>
            <Cd>CLBD</Cd>
          </CdOrPrtry>
        </Tp>
        <Amt Ccy=""EUR"">5367.93</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <Dt>
          <Dt>2025-12-24</Dt>
        </Dt>
      </Bal>
      <TxsSummry>
        <TtlCdtNtries>
          <NbOfNtries>4</NbOfNtries>
          <Sum>48.00</Sum>
        </TtlCdtNtries>
        <TtlDbtNtries>
          <NbOfNtries>0</NbOfNtries>
          <Sum>0.00</Sum>
        </TtlDbtNtries>
      </TxsSummry>
      <Ntry>
        <Amt Ccy=""EUR"">12.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-24</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-24</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000006-00000000006</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000006-00000000006</AcctSvcrRef>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">12.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DUMMY COMPANY C D.O.O.</Nm>
                  <PstlAdr>
                    <AdrLine>DUMMY BOULEVARD 36</AdrLine>
                    <AdrLine>DUMMYCITY</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000006</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR99</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy donation</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000007</NtryRef>
        <Amt Ccy=""EUR"">12.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-24</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-24</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000007-00000000007</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000007-00000000007</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0007</InstrId>
              <EndToEndId>XX000000000000-0007</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">12.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>JOHN DOE</Nm>
                  <PstlAdr>
                    <AdrLine>Dummy Square 92</AdrLine>
                    <AdrLine>Dummyville</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000007</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR0000000007</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy webinar payment</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000008</NtryRef>
        <Amt Ccy=""EUR"">12.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-24</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-24</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000008-00000000008</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000008-00000000008</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0008</InstrId>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">12.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DUMMY COMPANY D D.O.O.</Nm>
                  <PstlAdr>
                    <Ctry>HR</Ctry>
                    <AdrLine>DUMMY AVENUE 15</AdrLine>
                    <AdrLine>00000 DUMMYCITY</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000008</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR0000-00000000008</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>DONATION - Dummy seminar</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <Ntry>
        <NtryRef>00000000000009</NtryRef>
        <Amt Ccy=""EUR"">12.00</Amt>
        <CdtDbtInd>CRDT</CdtDbtInd>
        <RvslInd>false</RvslInd>
        <Sts>
          <Cd>BOOK</Cd>
        </Sts>
        <BookgDt>
          <Dt>2025-12-24</Dt>
        </BookgDt>
        <ValDt>
          <Dt>2025-12-24</Dt>
        </ValDt>
        <AcctSvcrRef>0000-000000009-00000000009</AcctSvcrRef>
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
              <AcctSvcrRef>0000-000000009-00000000009</AcctSvcrRef>
              <InstrId>DUMMYINSTRID0009</InstrId>
              <EndToEndId>HR99</EndToEndId>
            </Refs>
            <AmtDtls>
              <TxAmt>
                <Amt Ccy=""EUR"">12.00</Amt>
              </TxAmt>
            </AmtDtls>
            <RltdPties>
              <Dbtr>
                <Pty>
                  <Nm>DOE JANE</Nm>
                  <PstlAdr>
                    <Ctry>HR</Ctry>
                    <AdrLine>DUMMY STREET 3</AdrLine>
                    <AdrLine>00000 DUMMYCITY</AdrLine>
                  </PstlAdr>
                </Pty>
              </Dbtr>
              <DbtrAcct>
                <Id>
                  <IBAN>XX0000000000000000009</IBAN>
                </Id>
              </DbtrAcct>
            </RltdPties>
            <RmtInf>
              <Strd>
                <CdtrRefInf>
                  <Tp>
                    <CdOrPrtry>
                      <Cd>SCOR</Cd>
                    </CdOrPrtry>
                  </Tp>
                  <Ref>HR0000000009</Ref>
                </CdtrRefInf>
                <AddtlRmtInf>Dummy webinar payment 2.0</AddtlRmtInf>
              </Strd>
            </RmtInf>
          </TxDtls>
        </NtryDtls>
      </Ntry>
      <AddtlStmtInf>Privremeno stanje:              5.367,93; Rezervirano za naplatu:                  0,00; Dopušteno prekoračenje:                  0,00; Rezervirano po nalogu FINA-e:                  0,00; Raspoloživo stanje:              5.367,93</AddtlStmtInf>
    </Stmt>
  </BkToCstmrStmt>
</Document>";

        public const string htmlBankStatements = @"

<html xmlns:fo=""http://www.w3.org/1999/XSL/Format"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:fn=""http://www.w3.org/2005/xpath-functions"">
  <head>
    <META http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
    <style type=""text/css"">
          @media screen
          {
          body{ font-family : Verdana, serif; }
          div#THeadUp{ border-bottom: 1px solid white; border-top: 1px solid white; }
          div#THeadDown{ border-bottom: 1px solid white; }

          div#TItems{ border-bottom: 1px solid white; }
          div#TFootUp{ border-bottom: 1px solid white; }
          div#PageBreak{ padding-bottom: 30px; }

          .tbHeadUp{border-bottom: 1px solid white; border-top: 1px solid white;}
          .tbHeadDown{border-bottom: 1px solid white;}
          .trItems td{border-bottom: 1px solid white;}
          .trFootUp td{border-bottom: 1px solid white;}
          }

          @media print
          {
          body{ font-family : ""Arial"", Times, serif; }

          div#THeadUp{ border-bottom: 1px solid #000000; border-top: 1px solid #000000; }
          div#THeadDown{ border-bottom: 1px solid #000000; }
          div#TItems{ border-bottom: 1px solid #000000; }
          div#TFootUp{ border-bottom: 1px solid #000000; }
          div#PageBreak{ page-break-after: always; }

          .tbHeadUp{border-bottom: 1px solid #000000; border-top: 1px solid #000000;}
          .tbHeadDown{border-bottom: 1px solid #000000;}
          .trItems td{border-bottom: 1px solid #000000;}
          .trFootUp td{border-bottom: 1px solid #000000;}
          }

          div#Header{ font-size: 10pt; font-family:Arial}
          div#Generalno{font-size: 10pt}

          div#Generalno span{ display:inline-block; }

          div#Naslov p{ color : #000000; background : #ffffff; font-size : 14pt;}

          div#THeadUp
          {
          vertical-align: middle;
          color : #000000;
          background : #ffffff;
          font-size : 8pt;
          font-weight:normal;
          background-color:#CCE5F7;
          display:inline-block;
          }

          div#THeadUp span{display:inline-block;}

          div#THeadDown
          {
          color : #000000;
          background : #ffffff;
          font-size : 8pt;
          font-weight:bold;
          background-color:#CCE5F7;
          display:inline-block;
          }

          div#THeadDown span{display:inline-block;}

          div#TItems{
          color : #000000;
          background : #ffffff;
          font-size : 8pt;
          display:inline-block;
          }

          div#TItems span{
          page-break-inside: avoid;
          vertical-align:middle;
          display:inline-block;
          word-wrap:break-word;
          }

          div#TFootUp
          {
          text-align:center;
          color : #000000;
          background : #ffffff;
          font-size : 8pt;

          font-weight:bold;
          background-color:#CCE5F7;
          display:inline-block;

          }

          div#TFootUp span{display:inline-block;}

          div#TRekap{font-size : 8pt;page-break-inside: avoid;display:inline-block;padding-bottom: 3px}

          div#TRekap span{display:inline-block;}

          .tbHeadUp{color:#000000; background:#ffffff;font-size:8pt;font-weight:normal;background-color:#CCE5F7;width:100%;}
          .tbHeadDown{color:#000000; background:#ffffff;font-size:8pt;font-weight:bold;background-color:#CCE5F7;vertical-align:top;width:100%;}
          .trItems{color:#000000;background:#ffffff;font-size : 8pt;page-break-inside: avoid;}
          .trItems td{page-break-inside: avoid;vertical-align:middle;word-wrap:break-word;}
          .trFootUp{text-align:center;color:#000000;background:#ffffff;font-size:8pt;font-weight:bold;background-color:#CCE5F7;}
          .tbRekap {font-size: 8pt;page-break-inside: avoid;width:91%}
        </style>
  </head>
  <body style=""margin: 6px 6px 6px 6px;"">
    <div id=""Header"" style=""vertical-align:top;width:100%;"">
      <div id=""Naslov"" style=""vertical-align:top;width:100%"">
        <p>IZVOD PROMETA PO RAČUNU</p>
      </div><br><div>
              Datum i vrijeme izdavanja: 07.01.2026. 01:57</div>
      <div><span>
                Za razdoblje (po datumu obrade):
              </span><span style=""width:61%;text-align:left;"">06.01.2026.</span></div><br><div style=""width:100%;"">
        <table width=""100%"" cellpading=""0"" cellspacing=""0"">
          <tr>
            <td style=""width:50%; vertical-align:top;font-size:10pt"">
                    DUMMY BANK D.D.<br>
                    OIB: 00000000001<br>
                    SWIFT/BIC: DUMMHR22<br>10000 Zagreb, Ulica Banke 1<br>
                    Tel.: 000   000-000;
                    Faks.: 000   000-000<br>
                    www.dummybank.hr
                  </td>
            <td style=""width:50%; vertical-align:top"">
              <div id=""Right"" style=""width:100%"">
                <div id=""Generalno""><span style=""padding-left:30px;text-align:left;"">DUMMY ORGANIZATION<br>ULICA TESTNA 1 <br>10000 ZAGREB<br>REPUBLIKA HRVATSKA</span></div>
              </div>
            </td>
          </tr>
        </table>
      </div><br><div id=""Generalno"" style=""width:100%;""><span style=""width:120px;"">
                Naziv klijenta:
              </span><span style=""width:3%;text-align:left;""></span><span style=""width:61%;text-align:left;"">DUMMY ORGANIZATION</span></div>
      <div id=""Generalno"" style=""width:100%;""><span style=""width:120px;"">
                OIB:
              </span><span style=""width:3%;text-align:left;""></span><span style=""width:61%;text-align:left;"">00000000002</span></div><br><div id=""Generalno"" style=""width:100%;""><span style=""width:120px;"">
                IBAN:
              </span><span style=""width:3%;text-align:left;""></span><span style=""width:61%;text-align:left;"">HR0000000000000000001</span></div>
      <div id=""Generalno"" style=""width:100%;""><span style=""width:120px;text-align:left;"">
                Broj računa:
              </span><span style=""width:3%;text-align:left;""></span><span style=""width:61%;text-align:left;"">0000000001</span></div>
    </div><br><div id=""Generalno"" style=""width:100%;""><span style=""width:120px;text-align:left;"">
        Oznaka valute:
      </span><span style=""width:3%;text-align:left;""></span><span style=""width:61%;text-align:left;"">EUR</span></div>
    <div id=""Generalno"" style=""width:100%;""><span style=""width:120px;text-align:left;"">
        Broj izvoda:
      </span><span style=""width:3%;text-align:left;""></span><span style=""width:61%;text-align:left;"">005</span></div><br><table class=""tbHeadUp"" cellpading=""0"" cellspacing=""0"">
      <tr>
        <td style=""width:10%;text-align:left;vertical-align:top;"">
          Datum valute<br>
          Datum obrade
        </td>
        <td style=""width:29%;text-align:left;vertical-align:top;"">
          Platitelj/Primatelj <br>
          Broj računa/IBAN <br>
          Tečaj
        </td>
        <td style=""width:15%;text-align:left;vertical-align:top;"">
          Redni broj <br>
          Opis plaćanja <br>
          Šifra namjene
          </td>
        <td style=""width:26%;text-align:left;vertical-align:top;"">
          Poziv na broj platitelja <br>
          Poziv na broj primatelja <br>
          Referenca plaćanja
        </td>
        <td style=""width:10%;text-align:right;vertical-align:middle;"">
          Isplata
        </td>
        <td style=""width:10%;text-align:right;vertical-align:middle;padding-right:5px;"">
          Uplata
        </td>
      </tr>
    </table>
    <table class=""tbHeadDown"" cellpading=""0"" cellspacing=""0"">
      <tr>
        <td style=""width:10%;text-align:left;"">
          Početno stanje :
        </td>
        <td style=""width:29%;text-align:left;"">&nbsp;</td>
        <td style=""width:15%;text-align:left;"">&nbsp;</td>
        <td style=""width:26%;text-align:left;"">&nbsp;</td>
        <td style=""width:10%;text-align:left;"">&nbsp;</td>
        <td style=""width:10%;text-align:right;padding-right:5px;"">5.551,80</td>
      </tr>
    </table>
    <table cellpading=""0"" cellspacing=""0"" style=""width:100%"">
      <tr class=""trItems"" style=""width:100%;"">
        <td style=""width:10%;text-align:left;"">06.01.2026.<br>06.01.2026.</td>
        <td style=""width:29%;text-align:left;"">DUMMY COMPANY D.O.O. ZAGREB<br>HR0000000000000000002</td>
        <td style=""width:15%;text-align:left;"">1 - Dummy payment description</td>
        <td style=""width:26%;text-align:left;"">HR99 <br>HR00 00-00000000000<br>2026-0000000-00000000000</td>
        <td style=""width:10%;text-align:right;"">&nbsp;</td>
        <td style=""width:10%;text-align:right;padding-right:5px"">30,00</td>
      </tr>
      <tr class=""trFootUp"">
        <td colspan=""2"" style=""width:39%;text-align:left;vertical-align:top;"">
          Stanje na dan :
                        06.01.2026.</td>
        <td style=""width:15%;text-align:left;vertical-align:top;"">
          Broj izvoda
              005</td>
        <td style=""width:26%;text-align:center;vertical-align:top;""><span style=""display:inline-block; text-align:left"">
            Promet
            <br>S t a n j e
          </span></td>
        <td style=""width:10%;text-align:right;vertical-align:top;"">0,00<br> </td>
        <td style=""width:10%;text-align:right;padding-right:5px;vertical-align:top;"">30,00<br>5.581,80</td>
      </tr>
    </table>
    <table class=""tbHeadDown"" cellpading=""0"" cellspacing=""0"">
      <tr>
        <td style=""width:10%;text-align:left;"">
            Konačno stanje :
          </td>
        <td style=""width:29%;text-align:left;"">&nbsp;</td>
        <td style=""width:15%;text-align:left;"">&nbsp;</td>
        <td style=""width:26%;text-align:left;"">&nbsp;</td>
        <td style=""width:10%;text-align:left;"">&nbsp;</td>
        <td style=""width:10%;text-align:right;padding-right:5px;"">5.581,80</td>
      </tr>
    </table><br><table class=""tbRekap"" cellpading=""0"" cellspacing=""0"">
      <tr>
        <td colspan=""3"" style=""width:30%;text-align:left;background:#CCE5F7;font-weight:bold;"">
            R E K A P I T U L A C I J A
          </td>
        <td colspan=""6"">&nbsp;</td>
      </tr>
      <tr>
        <td colspan=""4"" style=""width:33%;"">&nbsp;</td>
        <td style=""width:15%;text-align:left;"">
            Prethodno stanje
          </td>
        <td style=""width:10%;text-align:right;"">5.551,80</td>
        <td style=""width:3%;"">&nbsp;</td>
        <td style=""width:20%;text-align:left;font-weight:bold;"">
            Privremeno stanje
          </td>
        <td style=""width:10%;text-align:right;font-weight:bold;"">5.581,80</td>
      </tr>
      <tr>
        <td style=""width:15%;text-align:left;"">
            Naloga na teret
          </td>
        <td style=""width:5%;text-align:right;"">0</td>
        <td colspan=""2"" style=""width:13%;"">&nbsp;</td>
        <td style=""width:15%;text-align:left;"">
            Dugovni promet
          </td>
        <td style=""width:10%;text-align:right;"">0,00</td>
        <td style=""width:3%;text-align:left;"">&nbsp;</td>
        <td style=""width:20%;text-align:left;text-wight:bold;"">
            Rezervirano za naplatu
          </td>
        <td style=""width:10%;text-align:right;text-wight:bold;"">0,00</td>
      </tr>
      <tr>
        <td style=""width:15%;text-align:left;"">
            Naloga u korist
          </td>
        <td style=""width:5%;text-align:right;"">1</td>
        <td colspan=""2"" style=""width:13%;text-align:left;"">&nbsp;</td>
        <td style=""width:15%;text-align:left;"">
            Potražni promet
          </td>
        <td style=""width:10%;text-align:right;"">30,00</td>
        <td style=""width:3%;text-align:left;"">&nbsp;</td>
        <td style=""width:20%;text-align:left;text-weight:bold;"">
            Dopušteno prekoračenje
          </td>
        <td style=""width:10%;text-align:right;text-weight:bold;"">0,00</td>
      </tr>
      <tr>
        <td colspan=""7"" style=""width:61%;"">&nbsp;</td>
        <td style=""width:20%;text-align:left;text-weight:bold;"">
            Rezervirano po nalogu FINA-e
          </td>
        <td style=""width:10%;text-align:right;text-weight:bold;"">0,00</td>
      </tr>
      <tr>
        <td style=""width:15%;text-align:left;"">
            Naloga ukupno
          </td>
        <td style=""width:5%;text-align:right;"">1</td>
        <td colspan=""2"" style=""width:13%;"">&nbsp;</td>
        <td style=""width:15%;text-align:left;"">
            Ukupni promet
          </td>
        <td style=""width:10%;text-align:right;"">30,00</td>
        <td colspan=""3"" style=""width:3%;text-align:left;"">&nbsp;</td>
      </tr>
      <tr>
        <td colspan=""7"" style=""width:61%;text-align:left;padding-bottom: 3px;border-bottom:1px solid #000000;"">&nbsp;</td>
        <td style=""width:20%;text-align:left;padding-bottom: 3px;border-bottom:1px solid #000000;"">
            Raspoloživo stanje
          </td>
        <td style=""width:10%;text-align:right;padding-bottom: 3px;border-bottom:1px solid #000000;"">5.581,80</td>
      </tr>
    </table>
    <div id=""TRekap"" style=""width:100%;margin-top:10px;""><span style=""width:100%;text-align:left;padding-bottom: 2px;"">
            STANJE OSTALIH RAČUNA PO POSLOVNOM RAČUNU NA DAN
             06.01.2026.</span></div>
    <div id=""TRekap"" style=""width:100%;""><span style=""width:30%;text-align:left;padding-bottom: 2px;"">
            Obračunata naknada
          </span><span style=""width:10%;text-align:right;padding-bottom: 2px;"">-37,97</span></div><br><br><div><span style=""width:100%;text-align:left;font-size : 8pt;""></span></div>
  </body>
</html>
";

    }
}
