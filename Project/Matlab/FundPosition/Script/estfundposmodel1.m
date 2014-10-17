function [ epos, bpos, IPOS, rsq, BETA] = estfundposmodel1(Y, X1, X2, step, type, start, X0)
%============================================
%估算基金的股票债券仓位和行业配置模型
%   --多项指数回归
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
%   指数回归法
%============================================
    %参数设置
    n=step;         %时间序列长度
    tposmax=0.95;   %股票和债券合计最高仓位
    switch type
        case 1  %股票型
            eposmin=0.6;	%股票最低仓位限制
            bposmax=0.4;	%债券最高仓位限制
        otherwise
            eposmin=0;
            bposmax=1;
    end
        
    %股票指数
    X1N = X1(start-n+1:start,:);
    [~,col1]=size(X1N);   
    
    %债券指数
    X2N = X2(start-n+1:start,1);
    [~,col2]=size(X2N);

    %自变量个数
    m=col1+col2;
            
    %构建不等式约束条件：  AX    <     B
    %   0	1  1 ...  1   1           tposmax
    %   0  -1 -1 ... -1   0          -eposmin
    %   0   0  0 ...  0   1           bposmax
    %   0  -1                         0
    %  ...    -1                      0
    %   0                -1           0
    
    A=[ ones(1,m);
       -ones(1,col1),zeros(1,col2);
        zeros(1,col1),ones(1,col2);
        -eye(m)
      ];
    A=[zeros(size(A),1),A];
    
    B=[ tposmax;
       -eposmin;
        bposmax;
        zeros(m,1);
        ];
    
    %构建回归变量:
    XN=[ones(n,1),X1N,X2N];  %第一列为全1，表示常数项
    YN=Y(start-n+1:start);

    %带约束的线性回归：最小二乘法
    options = optimset('MaxIter', 10000);
    [BETA,ssr] = lsqlin(XN,YN,A,B,[],[],[],[], X0,options);    
    
    %计算R^2
    sst=sum((YN-mean(YN)).^2);
    rsq = 1-ssr/sst;
%     rsq = 1-(n/(n-m))*(1-rsq);  %调整的R^2
    
    %行业配置系数
    IPOS = BETA(2:col1+1);

    %计算仓位
    epos = sum(IPOS);
    bpos = sum(BETA((1+col1+1):(1+m))); 
end

