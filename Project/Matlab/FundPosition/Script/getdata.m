function [ Y, X1, X2, P, S] = getdata( isgroup, indextype, N )
%获取数据
%   Y: 基金净值增长率
%   X1:股票指数增长率
%   X2:债券指数增长率
%   P: 真实季报仓位
%   S: 基金成立标志
%
%   isgroup:    1=将所有基金作为整体 0=单个基金
%   indextype:  作为参数的指数类型
%   N: 单个基金数据列数

     %测试数据
%     idata=csvread('..\data\index.csv',1,1);         %行业规模债券指数
%     fdata=csvread('..\data\fundnav.csv',1,1);       %基金净值增长率
%     sdata=csvread('..\data\fundsetup.csv',1,1);     %基金成立标志
%     pdata=csvread('..\data\fundposition.csv',1,1);  %季报公布仓位
%     fpxdata=csvread('..\data\fundpx.csv',1,1);      %基金净值
%     fshdata=csvread('..\data\fundshare.csv',1,1);   %基金份额（已经做21天延迟处理）
    
    %最新数据
    idata=csvread('..\data\index2.csv',1,1);         %行业规模债券指数
    fdata=csvread('..\data\fundnav2.csv',1,1);       %基金净值增长率
    pdata=csvread('..\data\fundposition2.csv',1,1);  %季报公布仓位
    fpxdata=csvread('..\data\fundpx2.csv',1,1);      %基金净值
    fshdata=csvread('..\data\fundshare2.csv',1,1);   %基金份额（已经做21天延迟处理）
    
    %===========================
    %数据
    %   按时间升序排列；各数据序列长度一致；长度最小240行（1年）
    %   样本内时间：2005/1/1-2011/12/31
    %   样本外时间：2012/1/1-2013/01/24
    %   indextype = 1: 申万一级行业指数 [1:23]
    %               2: Wind一级行业指数 [24:33]
    %               3: Wind证监会行业指数 [34:56]
    %               4: 中信一级行业指数 [57:85]
    %               5: 中证100/200/500规模指数 [86:88]
    %               6: 申万大/中/小规模指数 [89:91]
    %               7: 巨潮大/中/小规模指数 [94,96]
    %               8: 巨潮规模+风格指数 [97,102]
    %               9: 巨潮1000行业指数 [103,112]
    %               10:申万市盈率指数 [113,115]
    %               11:申万市净率指数 [116,118]
    %               12:申万股价指数 [119,121]
    %               13:申万盈利指数 [122,124]
    %               14:华泰行业指数 [125,147]
    %               15:中信风格指数 [148,152]
    %   债券指数: 中证全债 [92]
    %   转债指数: 中标可转债[93]    
    indextypelist=[1,23
     24,33
     34,56
     57,85
     86,88
     89,91
     94,96
     97,102
     103,112
     113,115
     116,118
     119,121
     122,124
     125,147
     148,152];
 
    if indextype==0
        X1=idata(:,N)/100;
    else
        X1=idata(:,indextypelist(indextype,1):indextypelist(indextype,2))/100;
    end
    
    X2=idata(:,92)/100;
    
    if isgroup==1
        %===========================
        %合并所有基金计算
        %===========================
        %计算各基金资产
        fasset=fpxdata.*fshdata;        
        totalshare=sum(fshdata,2);
        totalasset=sum(fasset,2);

        %计算各基金公布仓位
        feqp=sum(fpxdata.*fshdata.*pdata,2)./sum(fpxdata.*fshdata,2);

        %计算全部基金集合的收益率
        fappr=zeros(size(fasset),1);
        for fa=2:size(fasset)
            share1=totalshare(fa,1);
            share0=totalshare(fa-1,1);
            if share1==share0
                %季报之前假设份额不变
                fappr(fa)=totalasset(fa)/totalasset(fa-1)-1;
            else
                %季报之后份额变了
                fappr(fa)=(fpxdata(fa,:)*fshdata(fa,:)')/(fpxdata(fa-1,:)*fshdata(fa,:)')-1;
            end
        end
        
        Y=fappr(2:end);        
        P=feqp(2:end)/100;
        X1=X1(2:end,:);
        X2=X2(2:end,:);
        S=ones(size(Y));
    else
        %===========================
        %计算单个基金
        %===========================
        Y=fdata(:,N)/100;
        P=pdata(:,N)/100;
        S=sdata(:,N);
    end
    
    
end

