function [epos, IPOS, rsq] = model1(Y, X1, step, start, X0)
%============================================
%估算基金的股票债券仓位和行业配置模型
%   --多项指数回归
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
%   IPOS: 行业配置向量
%   rsq : 回归解释度
%模型：
%   指数回归法
%============================================
    %参数设置
    n=step;         %时间序列长度
    tposmax=0.95;   %股票和债券合计最高仓位
    eposmin=0;      %股票最低仓位限制
        
    %股票指数
    X1N = X1(start-n+1:start,:);
    [~,m]=size(X1N); 
            
    %构建不等式约束条件：  AX    <     B
    %   0	1  1 ...  1              tposmax
    %   0  -1 -1 ... -1             -eposmin
    %   0  -1                         0
    %  ...    -1                      0
    %   0            -1               0
    
    A=[ ones(1,m);
       -ones(1,m);
        -eye(m)
      ];
    A=[zeros(size(A),1),A];
    
    B=[ tposmax;
       -eposmin;
        zeros(m,1);
        ];
    
    %构建回归变量:
%     XN=[ones(n,1),X1N,X2N];  %第一列为全1，表示常数项
    XN=[ones(n,1),X1N];
    YN=Y(start-n+1:start);

    %带约束的线性回归：最小二乘法
    options = optimset('MaxIter', 10000);
    [BETA,ssr] = lsqlin(XN,YN,A,B,[],[],[],[], X0,options);    
    
    %计算R^2
    sst=sum((YN-mean(YN)).^2);
    rsq = 1-ssr/sst;
%     rsq = 1-(n/(n-m))*(1-rsq);  %调整的R^2
    
    %行业配置系数
    IPOS = BETA(2:m+1);

    %计算仓位
    epos = sum(IPOS);
end

