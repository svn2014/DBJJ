function [ epos, IPOS, rsq] = model2(Y, X1, step, start, X0)
%============================================
%估算基金的股票债券仓位和行业配置模型
%   --复合指数回归法
%============================================
%输入：
%[各输入参数按日期升序排列]
%[数值：5.23%记为5.23]
%   Y:  基金净值的收益率序列
%   X1: 行业指数的收益率序列
%   step: 测算步长
%   start: 当前运行位置
%   X0: 初始点
%输出：
%   epos: 股票仓位
%   IPOS: 规模配置向量
%   rsq : 回归解释度
%模型：
%   合成指数回归法
%============================================
    %参数设置
    n=step;         %时间序列长度

    %收益率
    XN=X1(start-n+1:start,:);
    YN=Y(start-n+1:start);
    [~,m]=size(XN);
    
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
    [B,~,~,~,STATS] = regress(YN,X);
    
    epos = B(2);
    IPOS=W*epos;
    rsq=STATS(1);
end

