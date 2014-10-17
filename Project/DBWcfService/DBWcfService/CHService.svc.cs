using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace DBWcfService
{
    public class CHService : ICHService
    {
        private DBInstanceOracle dbInstance = null;
        public CHService()
        {
            string connString = "Data Source=finchina1;User Id=caihui;Password=caihui;Integrated Security=no;";
            dbInstance = new DBInstanceOracle(connString);
        }

        public DataSet ExecuteSQL(string sql)
        {
            try
            {
                return dbInstance.ExecuteSQL(sql);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        public DataSet GetTradingDays(DateTime startDate, DateTime endDate)
        {
            string sql = @"Select tdate as trade_days, case exchange when 'CNSESH' then 'SSE' when 'CNSESZ' then 'SZSE' end as s_info_exchmarket
                            From TRADEDATE 
                            Where exchange = 'CNSESH' ";

            sql += " AND tdate >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND tdate <= '" + endDate.ToString("yyyyMMdd") + "' ";

            return ExecuteSQL(sql);
        }

        public DataSet GetStockPrices(DateTime startDate, DateTime endDate, string code)
        {
            //s_dq_volume: 手
            //s_dq_amount：千元
            string sql = @"Select A.Tdate as trade_dt
                            ,A.Symbol || case A.Exchange WHEN 'CNSESH' THEN '.SH' ELSE '.SZ' END as s_info_windcode
                            ,A.LCLOSE as s_dq_preclose
                            ,A.TOPEN as s_dq_open
                            ,A.HIGH as s_dq_high
                            ,A.Low as s_dq_low
                            ,A.TCLOSE as s_dq_close
                            ,A.VOTURNOVER/100 as s_dq_volume
                            ,A.VATURNOVER/1000 as s_dq_amount
                            ,A.AVGPRICE as s_dq_avgprice
                            ,A.Chg as s_dq_change,A.Pchg as s_dq_pctchange
                            ,B.Price2 as s_dq_adjclose
                            From SecurityCode S
                            INNER JOIN CHDQUOTE A
                                  ON S.Symbol = A.SYMBOL
                            Left join DERC_EQACHQUOTE_2 B
                                 On A.tdate = to_char(B.tdate,'yyyyMMdd') and A.Symbol=B.symbol and A.Exchange = B.Exchange
                            Where exchange in ('CNSESH','CNSESZ') 
                            And Stype = 'EQA' ";

            sql += " AND A.tdate >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND A.tdate <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "'";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetIndexPrices(DateTime startDate, DateTime endDate, string code)
        {
            string sql = @"SELECT 
                            TDate as trade_dt,
                            SYMBOL || case exchange when 'CNSESH' THEN '.SH' when 'CNSESZ' THEN '.SZ' END as s_info_windcode,
                            LCLOSE as s_dq_preclose,
                            TOPEN as s_dq_open,
                            HIGH as s_dq_high,
                            LOW as s_dq_low,
                            TCLOSE as s_dq_close,
                            VOTURNOVER as s_dq_volume,
                            VATURNOVER as s_dq_amount,
                            pchg as s_dq_pctchange
                            FROM CIHDQUOTE A
                            WHERE 1=1 ";

            sql += " AND A.tdate >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND A.tdate <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "'";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetAShareStockList(string code, string exchange, string charIsST, string charIsActive)
        {
            string sql = @"SELECT A.SYMBOL || case A.exchange when 'CNSESH' then '.SH' when 'CNSESZ' then '.SZ' end as S_INFO_WINDCODE
                            ,A.SNAME as S_INFO_NAME
                            ,case A.exchange when 'CNSESH' then 'SSE' when 'CNSESZ' then 'SZSE' end as s_info_exchmarket
                            ,case when A.LISTDATE = to_date('19000101','yyyyMMdd') THEN null else to_char(A.LISTDATE,'yyyyMMdd') end as s_info_listdate
                            ,case when A.ENDDATE = to_date('19000101','yyyyMMdd') THEN null else to_char(A.ENDDATE,'yyyyMMdd') end as s_info_delistdate
                            ,CASE WHEN A.SNAME LIKE '%ST%' THEN '1' ELSE '0' END as ST 
                            ,CASE WHEN (A.LISTDATE > to_date('19000101','yyyyMMdd') AND (A.ENDDATE = to_date('19000101','yyyyMMdd') OR A.ENDDATE IS NULL)) THEN '1' ELSE '0' END AS Active
                            , B.Cindustry2 as sw_ind_name1
                            , C.Cindustry2 as sw_ind_name2
                            , D.Cindustry2 as sw_ind_name3
                            , case when E.Symbol is null then null else E.Symbol || '.SI' end as s_info_indexcode
                            FROM SECURITYCODE A
                            LEFT JOIN CINDUSTRY B
                                 ON A.Companycode = B.Companycode AND B.Style = '707' AND substr(B.StyleCode,3,4)='0000'
                            LEFT JOIN CINDUSTRY C
                                 ON A.Companycode = C.Companycode AND C.Style = '707' AND substr(C.StyleCode,3,4)!='0000' AND substr(C.StyleCode,5,2)='00'
                            LEFT JOIN CINDUSTRY D
                                 ON A.Companycode = D.Companycode AND D.Style = '707' AND substr(D.StyleCode,3,4)!='0000' AND substr(D.StyleCode,5,2)!='00'
                            LEFT JOIN Iindex_Comp E
                                 ON B.Stylecode = E.Industrycode AND E.Typecode = '3'
                            WHERE STYPE = 'EQA' ";

            code = (code == null) ? "" : code;
            charIsST = (charIsST == null) ? "" : charIsST;
            charIsActive = (charIsActive == null) ? "" : charIsActive;

            string exchangeCodeCH = "";
            switch (exchange)
            {
                case "SSE":
                    exchangeCodeCH = "CNSESH";
                    break;
                case "SZSE":
                    exchangeCodeCH = "CNSESZ";
                    break;
                default:
                    exchangeCodeCH = "";
                    break;
            }

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "'";

            if (exchangeCodeCH.Length > 0)
                sql += " AND A.exchange = '" + exchangeCodeCH;

            if (charIsST == "1")
                sql += " AND A.SNAME LIKE '%ST%' ";
            else if (charIsST == "0")
                sql += " AND NOT (A.SNAME LIKE '%ST%') ";

            if (charIsActive == "1")
                sql += " AND (A.LISTDATE > to_date('19000101','yyyyMMdd') AND (A.ENDDATE = to_date('19000101','yyyyMMdd') OR A.ENDDATE IS NULL))";
            else if (charIsActive == "0")
                sql += " AND NOT (A.LISTDATE > to_date('19000101','yyyyMMdd') AND (A.ENDDATE = to_date('19000101','yyyyMMdd') OR A.ENDDATE IS NULL))";
                        
            return ExecuteSQL(sql);
        }

        public DataSet GetIndexWeights(string code, DateTime monthEndDate)
        {
            //monthEndDate已经废弃不用

            //DateTime dMonthEnd = monthEndDate.AddDays(1);
            //dMonthEnd = new DateTime(dMonthEnd.Year, dMonthEnd.Month, 1).AddDays(-1);

            string sql = @"select To_Char(W.tdate,'yyyyMMdd') as trade_dt
                            , W.symbol || case W.exchange when 'CNSESH' then '.SH' when 'CNSESZ' then '.SZ' end as s_con_windcode
                            , W.weighing as i_weight
                            , case when E.Symbol is null then null else E.Symbol || '.SI' end as s_info_indexcode
                            , B.Cindustry2 as industriesname
                            from issweight_month W
                            INNER JOIN SECURITYCODE A
                                 ON W.Symbol = A.Symbol AND W.Exchange = A.Exchange
                            LEFT JOIN CINDUSTRY B
                                   ON A.Companycode = B.Companycode AND B.Style = '707' AND substr(B.StyleCode,3,4)='0000'
                            LEFT JOIN Iindex_Comp E
                                   ON B.Stylecode = E.Industrycode AND E.Typecode = '3'
                            where 1=1 ";

            sql += " AND W.TDATE = (SELECT MAX(TDATE) FROM issweight_month where isymbol= W.isymbol) ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND W.isymbol='" + code.Substring(0, 6) + "'";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetSWIndustryList()
        {
            string sql = @"select b.symbol || '.SI' as s_info_indexcode, a.CINDUSTRY2 as  industriesname
                            From
                            (
                            select distinct stylecode,CINDUSTRY2 from CINDUSTRY where style='707' AND substr(StyleCode,3,4)='0000'
                            )a
                            left join Iindex_Comp B
                            on a.stylecode=b.IndustryCode and B.typecode='3'
                            ORDER BY b.symbol ";

            return ExecuteSQL(sql);
        }

        public DataSet GetFreeFloatCapital(DateTime endDate, string code)
        {
            string sql = @"SELECT A.SYMBOL || case A.exchange when 'CNSESH' then '.SH' when 'CNSESZ' then '.SZ' end as S_INFO_WINDCODE
                            ,A.SNAME as S_INFO_NAME
                            ,to_char(B.Publishdate,'yyyyMMdd') as Change_Dt
                            ,B.SCSTC1 as s_share_totalshares --总股本
                            ,B.Scstc26 as s_share_freeshares--流通股本
                            ,B.SCSTC27 as s_share_freeAshares--已流通A股
                            FROM SECURITYCODE A
                            INNER JOIN SCSTC B
                                  ON A.Companycode = B.Companycode
                            INNER JOIN (
                                  select Companycode, MAX(Publishdate) as Publishdate
                                  from SCSTC 
                                  where Publishdate <= to_date('" + endDate.ToString("yyyyMMdd") + @"','yyyyMMdd')
                                  group by Companycode
                            ) C   ON A.Companycode = C.Companycode AND B.Publishdate = C.Publishdate
                            WHERE A.Stype = 'EQA' ";
            
            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "'";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetChinaBondList(string code, string exchange)
        {
            string sql = @"SELECT * FROM (
                              select bonddt16 || '.SH' as s_info_windcode
                              , Abname as s_info_name, 'SSE' as s_info_exchmarket, bonddt18 as cb_windcode
                              , to_char(BONDDT20,'yyyyMMdd') as b_info_maturitydate, BONDDT20 as b_info_maturitydated
                              from BONDDT where bonddt16 is not null
                              union
                              select bonddt17 || '.SZ' as s_info_windcode
                              , Abname as s_info_name, 'SZSE' as s_info_exchmarket, bonddt18 as cb_windcode
                              , to_char(BONDDT20,'yyyyMMdd') as b_info_maturitydate, BONDDT20 as b_info_maturitydated
                              from BONDDT where bonddt17 is not null
                              union
                              select bonddt47 || '.IB' as s_info_windcode
                              , Abname as s_info_name, 'NIB' as s_info_exchmarket, bonddt18 as cb_windcode
                              , to_char(BONDDT20,'yyyyMMdd') as b_info_maturitydate, BONDDT20 as b_info_maturitydated
                              from BONDDT where bonddt47 is not null
                            ) A
                            WHERE 1=1 ";
            
            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.s_info_windcode = '" + code + "'";

            if (exchange == null)
                exchange = "";

            if (exchange.Length > 0)
                sql += " AND A.s_info_exchmarket = '" + exchange + "'";

            return ExecuteSQL(sql);
        }

        public DataSet GetBlockTrade(DateTime startDate, DateTime endDate, string code)
        {
            string sql = @"select to_char(publishdate,'yyyyMMdd')  as  trade_dt, publishdate as trade_dtd
                            , symbol || case exchange when 'CNSESH' then '.SH' when 'CNSESZ' then '.SZ' end as s_info_windcode
                            , sname as s_info_name
                            , dzjy5 as s_block_price
                            , dzjy2 as s_block_volume
                            , DZJY6 as s_block_amount
                            , DZJY9 as s_block_buyername
                            , DZJY11 as s_block_sellername
                            from DZJY A
                            where STYPE in('EQA') ";

            sql += " AND to_char(A.publishdate,'yyyyMMdd') >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND to_char(A.publishdate,'yyyyMMdd') <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "'";

            return ExecuteSQL(sql);
        }

        public DataSet GetMarginTrade(DateTime startDate, DateTime endDate, string code)
        {
            string sql = @"select TDate as trade_dt, to_date(TDate,'yyyyMMdd') as trade_dtd
                            , A.symbol || case A.exchange when 'CNSESH' then '.SH' when 'CNSESZ' then '.SZ' end as s_info_windcode
                            , B.Sname as s_info_name
                            ,SEFTrade1/10000 as s_margin_tradingbalance
                            ,SEFTrade2/10000 as s_margin_purchwithborrowmoney
                            ,SEFTrade3/10000 as s_margin_repaymenttobroker
                            ,SEFTrade4/10000 as s_margin_seclendingbalance
                            ,SEFTrade5/10000 as s_margin_seclendingbalancevol
                            ,SEFTrade6/10000 as s_margin_salesofborrowedsec
                                ,C.TCLOSE*SEFTrade6/10000 as s_margin_salesofborrowedamt
                            ,SEFTrade7/10000 as s_margin_repaymentofborrowsec
                                ,C.TCLOSE*SEFTrade7/10000 as s_margin_repaymentofborrowamt
                            ,SEFTrade8/10000 as s_margin_margintradebalance
                            from SEFTrade A
                            INNER JOIN SECURITYCODE B
                                  ON A.Symbol = B.Symbol AND A.Exchange = B.Exchange 
                            LEFT JOIN CHDQUOTE C
                                 ON A.Symbol = C.Symbol AND A.Exchange = C.Exchange AND A.Tdate = C.Tdate
                            WHERE 1=1 ";

            sql += " AND TDate >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND TDate <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "' ";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetMarketNews(DateTime startDate, DateTime endDate, string codelist)
        {
            string sql = @"SELECT A.Fincode ||case C.Exchange when 'CNSESH' then '.SH' when 'CNSESZ' then '.SZ' end as s_info_windcode
                            , C.Sname as s_info_name
                            , to_char(B.Publishdate,'yyyyMMdd') as trade_dt, B.Publishdate as trade_dtd
                            , B.TITLE
                            , B.Mediumname
                            , A.NEWSTEXTID
                            FROM NewsFin A
                            INNER JOIN NewsText B
                                  ON A.Newstextid = B.Newstextid
                            INNER JOIN SecurityCode C
                                  ON A.Fincode = C.Symbol
                            WHERE A.Fintype in ('2') AND C.Stype = 'EQA' ";

            sql += " AND to_char(B.publishdate,'yyyyMMdd') >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND to_char(B.publishdate,'yyyyMMdd') <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (codelist == null)
                codelist = "";
            else
            {
                //e.g. codelist = "600001.SH, 000001.SZ", 以逗号分割
                string[] codeArray = codelist.Split(",".ToCharArray());
                string filter = " AND C.Symbol IN (";
                foreach (string code in codeArray)
                {
                    filter += "'" + code.Substring(0, 6) + "',";
                }
                filter = filter.Substring(0, filter.Length - 1);
                filter += ") ";
                sql += filter;
            }

            //Order by 
            sql += " ORDER BY B.publishdate DESC, C.Symbol";

            return ExecuteSQL(sql);
        }

        public DataSet GetMarketNewsContent(string newsId)
        {
            string sql = @"SELECT * 
                            FROM NewsText 
                            WHERE Newstextid = '" + newsId + "' ";
            
            return ExecuteSQL(sql);
        }
        
        public DataSet GetStrangeTrade(DateTime startDate, DateTime endDate, string code)
        {
            string sql = @"select A.tdate as trade_dtd
                            , A.symbol || CASE B.Exchange WHEN 'CNSESH' THEN '.SH' WHEN 'CNSESZ' THEN '.SZ' END as s_info_windcode
                            , B.Sname as s_info_name
                            , CASE  
                                WHEN SMEBTCOMPANY1 in ('01','02','03','04','10','11','12','13','14','17','18','26','27','28','29','30','31') THEN to_char(A.tdate,'yyyyMMdd')
                                WHEN SMEBTCOMPANY1 in ('19','20') THEN to_char(A.tdate-1,'yyyyMMdd')
                                ELSE to_char(A.tdate-2,'yyyyMMdd')
                              END as s_strange_bgdate
                            , to_char(A.tdate,'yyyyMMdd') as s_strange_enddate
                            , CASE  
                                WHEN SMEBTCOMPANY1 in ('01','02','03','04','10','11','12','13','14','17','18','26','27','28','29','30','31') THEN '1日'
                                WHEN SMEBTCOMPANY1 in ('19','20') THEN '2日'
                                ELSE '3日'
                              END as s_strange_type
                            , Case When SMEBTCOMPANY4 - SMEBTCOMPANY5>0 THEN 'B' ELSE 'S' end as s_strange_trade
                            , B.Pchg/100 as s_strange_range
                            , B.Voturnover/10000 as s_strange_volume   --万股
                            , B.Vaturnover/10000 as s_strange_amount   --万元
                            , SMEBTCOMPANY3 as s_strange_tradername
                            , SMEBTCOMPANY4/10000 as s_strange_buyamount   --万元
                            , SMEBTCOMPANY5/10000 as s_strange_sellamount  --万元
                            , B.Avgprice as s_dq_avgprice
                            from smebtcompany A
                            INNER JOIN CHDQUOTE B
                                  ON A.Symbol = B.Symbol AND A.Tdate = to_date(B.Tdate,'yyyyMMdd')
                            where B.Exchange IN ('CNSESH','CNSESZ')
                             ";

            sql += " AND B.TDate >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND B.TDate <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND A.Symbol = '" + code.Substring(0, 6) + "' ";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetRestrictStock(DateTime startDate, DateTime endDate, string code)
        {
            string sql = @"Select B.Symbol || CASE B.Exchange WHEN 'CNSESH' THEN '.SH' WHEN 'CNSESZ' THEN '.SZ' END as s_info_windcode
                            , B.Sname as s_info_name
                            , DECLAREDATE as ann_dtd, to_char(DECLAREDATE,'yyyyMMdd') as ann_dt
                            , SSTMHDLIST5 as s_info_listdated, to_char(SSTMHDLIST5,'yyyyMMdd') as s_info_listdate   --上市日期
                            , SSTMHDLIST6/10000 as s_share_lst       --新增可上市股份数量
                            , CASE WHEN SSTMHDLIST8=0 THEN NULL ELSE SSTMHDLIST8/10000 END as s_share_nonlst    --剩余有限售股份数量
                            , case SSTMHDLIST9 when 1 then '预案' when 2 then '实施' when 3 then '预案(未实施)' end as s_share_lstsym    --方案特征 
                            , C.TCLOSE*SSTMHDLIST6/10000 as s_share_lst_amount --新增可上市股份金额（万元）
                            , CASE WHEN SSTMHDLIST8=0 THEN NULL ELSE C.TCLOSE*SSTMHDLIST8/10000 END as s_share_nonlst_amount
                            , CASE SSTMHDLIST1 
                              WHEN 1 THEN '股权分置'
                              WHEN 2 THEN '首发'
                              WHEN 3 THEN '增发'
                              WHEN 4 THEN '配股'
                              WHEN 5 THEN '股权转让'
                              WHEN 6 THEN '合并'
                              WHEN 7 THEN '追加限售'
                              WHEN 8 THEN '股权激励'
                              WHEN 9 THEN '其它'
                              END as s_share_lsttypetext
                            From sstmhdlist A
                            INNER JOIN securitycode B
                                  ON A.Companycode = B.Companycode
                            LEFT JOIN CHDQUOTE C
                                  ON B.Symbol = C.Symbol
                            INNER JOIN (select MAX(TDATE) as TDATE from TRADEDATE where exchange = 'CNSESH' and tdate < to_char(sysdate-1,'yyyyMMdd')) D
                                  ON C.TDATE = D.TDate      
                            WHERE A.Companycode = SSTMHDLIST2 
                            AND SSTMHDLIST9 in (1,2) ";

            sql += " AND to_char(SSTMHDLIST5,'yyyyMMdd') >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND to_char(SSTMHDLIST5,'yyyyMMdd') <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND B.Symbol = '" + code.Substring(0, 6) + "' ";

            return ExecuteSQL(sql);
        }
        
        public DataSet GetIPOSEO(DateTime startDate, DateTime endDate, string code)
        {
            string sql = @"Select to_char(A.Declaredate,'yyyyMMdd') as s_fellow_offeringdate, A.Declaredate as s_fellow_offeringdated
                            ,B.Symbol || CASE A.Exchange WHEN 'CNSESH' THEN '.SH' WHEN 'CNSESZ' THEN '.SZ' END as s_info_windcode 
                            , B.sname as s_info_name
                            , case SIBASEINFO4 
                                   when 0 then '439006000'    --非公开
                                   when -1 then '439010000'   --公开
                              END as s_fellow_issuetype
                            , SIBASEINFO8 as s_fellow_price
                            , SIBASEINFO10 as s_fellow_amount
                            , SIBASEINFO12 as s_fellow_collection
                            , case SIBASEINFO4 
                                   when 0 then '非公开'
                                   when -1 then '公开'
                              END || '-' ||
                              CASE FTYPE 
                                   WHEN '01' THEN '首发'
                                   WHEN '02' THEN '增发'
                                   WHEN '06' THEN '增发'
                                   WHEN '03' THEN '配股'
                                   ELSE '其他'
                              END || '-' ||
                              CASE substr(SIBASEINFO1,1,1)
                                   WHEN 'A' THEN '战略配售' 
                                   WHEN 'B' THEN '定向配售' 
                                   ELSE '网上网下'
                              END as s_fellow_issuetypetext
                            from SIBASEINFO A
                            INNER JOIN SecurityCode B
                                  ON A.Companycode = B.Companycode
                            where A.Stype = 'EQA' AND B.Stype = 'EQA' ";

            sql += " AND to_char(A.Declaredate,'yyyyMMdd') >= '" + startDate.ToString("yyyyMMdd") + "' ";
            sql += " AND to_char(A.Declaredate,'yyyyMMdd') <= '" + endDate.ToString("yyyyMMdd") + "' ";

            if (code == null)
                code = "";

            if (code.Length >= 6)
                sql += " AND B.Symbol = '" + code.Substring(0, 6) + "'";

            return ExecuteSQL(sql);
        }

        public DataSet GetChinaBondsList(string codeList)
        {
            //e.g.: codeList = "600000.SH, 000001.SZ"
            string sql = @"SELECT * FROM (
                              select bonddt16 || '.SH' as s_info_windcode
                              , Abname as s_info_name, 'SSE' as s_info_exchmarket, bonddt18 as cb_windcode
                              , to_char(BONDDT20,'yyyyMMdd') as b_info_maturitydate, BONDDT20 as b_info_maturitydated
                              from BONDDT where bonddt16 is not null
                              union
                              select bonddt17 || '.SZ' as s_info_windcode
                              , Abname as s_info_name, 'SZSE' as s_info_exchmarket, bonddt18 as cb_windcode
                              , to_char(BONDDT20,'yyyyMMdd') as b_info_maturitydate, BONDDT20 as b_info_maturitydated
                              from BONDDT where bonddt17 is not null
                              union
                              select bonddt47 || '.IB' as s_info_windcode
                              , Abname as s_info_name, 'NIB' as s_info_exchmarket, bonddt18 as cb_windcode
                              , to_char(BONDDT20,'yyyyMMdd') as b_info_maturitydate, BONDDT20 as b_info_maturitydated
                              from BONDDT where bonddt47 is not null
                            ) A
                            WHERE 1=1 ";

            string strCodeWhere = "";
            if (codeList == null)
                codeList = "";
            else
            {
                string[] codes = codeList.Split(",".ToCharArray());

                foreach (string code in codes)
                {
                    strCodeWhere += "'" + code + "',";
                }

                strCodeWhere = strCodeWhere.Substring(0, strCodeWhere.Length - 1);
            }

            if (strCodeWhere.Length >= 6)
                sql += " AND A.s_info_windcode in (" + strCodeWhere + ")";

            return ExecuteSQL(sql);
        }
    }
}
