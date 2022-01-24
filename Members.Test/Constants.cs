namespace Members.Test
{
    class Constants
    {
        public const string jsonbnkstatement= @"
{
	""Data"": [
		{
			""BankID"": 29,
			""Bank"": ""ERSTE"",
			""StatementID"": 1,
			""StatementNumber"": 1,
			""Number"": ""2020-19206899-8688860902"",
			""PaymentDescription"": ""DONACIJA"",
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
					""City"": ""PUT BANA J"",
					""CountryID"": 385,
					""Country"": null
				},
				""Account"": ""HR44444""
			}
		},
		{
			""BankID"": 29,
			""Bank"": ""ERSTE"",
			""StatementID"": 1,
			""StatementNumber"": 1,
			""Number"": ""2020-19221230-8688907662"",
			""PaymentDescription"": ""DONACIJA"",
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
					""City"": ""SKOKOV PRI"",
					""CountryID"": 385,
					""Country"": null
				},
				""Account"": ""HR111111""
			}
		}
	],
	""Count"": 2
}";

		public const string xmlbnkstatmnt = @"
<Document xmlns =""urn:iso:std:iso:20022:tech:xsd:camt.053.001.02"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:fo=""http://www.w3.org/1999/XSL/Format"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"" xmlns:fn=""http://www.w3.org/2005/xpath-functions"">
	<BkToCstmrStmt>
		<GrpHdr>
			<MsgId>053.110097095419120211217.0001</MsgId>
			<CreDtTm>2021-12-17T06:50:12.786</CreDtTm>
		</GrpHdr>
		<Stmt>
			<Id>CAMT053/1100970954/191/2021/221/001</Id>
			<LglSeqNb>221</LglSeqNb>
			<CreDtTm>2021-12-17T06:50:12.786</CreDtTm>
			<FrToDt>
				<FrDtTm>2021-12-16T00:00:00.000</FrDtTm>
				<ToDtTm>2021-12-16T23:59:59.999</ToDtTm>
			</FrToDt>
			<RptgSrc>
				<Prtry>ESBCHR2223057039320</Prtry>
			</RptgSrc>
			<Acct>
				<Id>
					<IBAN>HR2324020061100970954</IBAN>
				</Id>
				<Ccy>HRK</Ccy>
				<Nm>GLAS PODUZETNIKA</Nm>
				<Svcr>
					<FinInstnId>
						<BIC>ESBCHR22</BIC>
					</FinInstnId>
				</Svcr>
			</Acct>
			<Bal>
				<Tp>
					<CdOrPrtry>
						<Cd>OPBD</Cd>
					</CdOrPrtry>
				</Tp>
				<Amt Ccy=""HRK"">695786.04</Amt>
				<CdtDbtInd>CRDT</CdtDbtInd>
				<Dt>
					<Dt>2021-12-16</Dt>
				</Dt>
			</Bal>
			<Bal>
				<Tp>
					<CdOrPrtry>
						<Cd>CLBD</Cd>
					</CdOrPrtry>
				</Tp>
				<Amt Ccy=""HRK"">699986.04</Amt>
				<CdtDbtInd>CRDT</CdtDbtInd>
				<Dt>
					<Dt>2021-12-16</Dt>
				</Dt>
			</Bal>
			<TxsSummry>
				<TtlCdtNtries>
					<NbOfNtries>2</NbOfNtries>
					<Sum>4200.00</Sum>
				</TtlCdtNtries>
				<TtlDbtNtries>
					<NbOfNtries>0</NbOfNtries>
					<Sum>0.00</Sum>
				</TtlDbtNtries>
			</TxsSummry>
			<Ntry>
				<NtryRef>M160213500094472</NtryRef>
				<Amt Ccy=""HRK"">200.00</Amt>
				<CdtDbtInd>CRDT</CdtDbtInd>
				<RvslInd>false</RvslInd>
				<Sts>BOOK</Sts>
				<BookgDt>
					<Dt>2021-12-16</Dt>
				</BookgDt>
				<ValDt>
					<Dt>2021-12-16</Dt>
				</ValDt>
				<AcctSvcrRef>2021-75752768-9734965634</AcctSvcrRef>
				<BkTxCd>
					<Domn>
						<Cd>PMNT</Cd>
						<Fmly>
							<Cd>RCDT</Cd>
							<SubFmlyCd>NTAV</SubFmlyCd>
						</Fmly>
					</Domn>
				</BkTxCd>
				<NtryDtls>
					<TxDtls>
						<Refs>
							<AcctSvcrRef>2021-75752768-9734965634</AcctSvcrRef>
							<InstrId>2402006211216149003C415100800269B3</InstrId>
							<EndToEndId>HR99</EndToEndId>
						</Refs>
						<AmtDtls>
							<TxAmt>
								<Amt Ccy=""HRK"">200.00</Amt>
								<CcyXchg>
									<SrcCcy>HRK</SrcCcy>
									<TrgtCcy>HRK</TrgtCcy>
									<XchgRate>1.000000</XchgRate>
								</CcyXchg>
							</TxAmt>
						</AmtDtls>
						<RltdPties>
							<Dbtr>
								<Nm>Dont tell me</Nm>
								<PstlAdr>
									<AdrLine>Adresa1</AdrLine>
								</PstlAdr>
							</Dbtr>
							<DbtrAcct>
								<Id>
									<IBAN>HR8823600003211771899</IBAN>
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
									<Ref>HR6900000-222</Ref>
								</CdtrRefInf>
								<AddtlRmtInf>DONACIJA</AddtlRmtInf>
							</Strd>
						</RmtInf>
					</TxDtls>
				</NtryDtls>
			</Ntry>
			<Ntry>
				<Amt Ccy=""HRK"">360.00</Amt>
				<CdtDbtInd>DBIT</CdtDbtInd>
				<RvslInd>false</RvslInd>
				<Sts>BOOK</Sts>
				<BookgDt>
					<Dt>2022-01-05</Dt>
				</BookgDt>
				<ValDt>
					<Dt>2022-01-05</Dt>
				</ValDt>
				<AcctSvcrRef>2022-00658308-9771600324</AcctSvcrRef>
				<BkTxCd>
					<Domn>
						<Cd>PMNT</Cd>
						<Fmly>
							<Cd>ICDT</Cd>
							<SubFmlyCd>NTAV</SubFmlyCd>
						</Fmly>
					</Domn>
				</BkTxCd>
				<NtryDtls>
					<TxDtls>
						<Refs>
							<AcctSvcrRef>2022-00658308-9771600324</AcctSvcrRef>
							<EndToEndId>HR99</EndToEndId>
						</Refs>
						<AmtDtls>
							<TxAmt>
								<Amt Ccy = ""HRK""> 360.00 </Amt>
								<CcyXchg>
									<SrcCcy> HRK </SrcCcy>
									<TrgtCcy> HRK </TrgtCcy>
									<XchgRate> 1.000000 </XchgRate>
								</CcyXchg>
							</TxAmt>
						</AmtDtls>
						<RltdPties>
							<Cdtr>
								<Nm>Drugi</Nm>
								<PstlAdr>
									<AdrLine>Drugi Adresa</AdrLine>
								</PstlAdr>
							</Cdtr>
							<CdtrAcct>
								<Id>
									<IBAN>HR2023400093235384955</IBAN>
								</Id>
							</CdtrAcct>
						</RltdPties>
						<Purp>
							<Cd>OTHR</Cd>
						</Purp>
						<RmtInf>
							<Strd>
								<CdtrRefInf>
									<Tp>
										<CdOrPrtry>
											<Cd>SCOR</Cd>
										</CdOrPrtry>
									</Tp>
									<Ref>HR6940002-87582274224-190</Ref>
								</CdtrRefInf>
								<AddtlRmtInf>Naknada</AddtlRmtInf>
							</Strd>
						</RmtInf>
					</TxDtls>
				</NtryDtls>
			</Ntry>
			<AddtlStmtInf>Privremeno stanje:699.986,04; Rezervirano za naplatu:0,00; Dopušteno prekoraèenje:0,00; Rezervirano po nalogu FINA-e:0,00; Raspoloživo stanje:699.986,04</AddtlStmtInf>
		</Stmt>
	</BkToCstmrStmt>
</Document>
";
	}
}
