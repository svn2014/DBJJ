function [ epos, bpos, IPOS, rsq] = estfundposmodel2(Y, X1, X2, step, type, start)
%============================================
%估算基金的股票债券仓位和行业配置模型
%   --复合指数回归法
%============================================
%输入：
%[各输入参数按日期升序排列]
%[数值：5.23%记为5.23]
%   Y:  基金净值的收益率序列
%   X1: 行业指数的收益率序列
%   X2: 债券指数的收益率序列
%   step: 时间序列长度
%   type: 基金类型；1=股票型，2=混合型
%   start: 当前运行位置
%输出：
%   epos: 股票仓位
%   bpos: 债券仓位
%   IPOS: 行业配置向量
%   rsq : 回归解释度
%模型：
%   合成指数回归法
%============================================
    %参数设置
    n=step;         %时间序列长度
        
    %股票指数
    X1N = X1(start-n+1:start,:);
    [~,col1]=size(X1N);
%     %债券指数
%     X2N = X2(x0-n+1:x0,1);
%     [~,col2]=size(X2N);
%     %全部指数
%     XN=[X1N,X2N];
    XN=X1N;

    %基金收益率
    YN=Y(start-n+1:start);
    
%     %自变量个数
%     m=col1+col2;
    m=col1;
    
    %构建二次优化: 
    H=2*(XN')*XN;
    F=-2*(YN')*XN;    
    A=-eye(m);
    B=zeros(m,1);
    Aeq=ones(1,m);
    Beq=1;
    W=quadprog(H,F,A,B,Aeq,Beq);
    
    %根据二次优化构建回归变量:
    X=[ones(n,1),XN*W];  %第一列为全1，表示常数项
    [B,BINT,R,RINT,STATS] = regress(YN,X);
    
    epos = B(2);
    bpos = 0.95-epos;
    IPOS=W*epos;
    rsq=STATS(1);

end

