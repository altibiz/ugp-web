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

    }
}
