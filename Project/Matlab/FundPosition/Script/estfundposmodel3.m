function [ epos, bpos, IPOS, rsq] = estfundposmodel3(Y, X1, X2, step, type, start)
%============================================
%估算基金的股票债券仓位和行业配置模型
%   --主成份回归法
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
%   指数主成份回归法
%============================================
    %参数设置
    n=step;            %时间序列长度
    tposmax=0.95;      %股票和债券合计最高仓位
    reqvecnum=30;       %要求特征向量数量
    reqexplained=90;   %要求累计解释度, 当reqvecnum=0时有效
        
    switch type
        case 1  %股票型
            eposmin=0.6;	%股票最低仓位限制
            bposmax=0.4;	%债券最高仓位限制
        otherwise
            eposmin=0;
            bposmax=1;
    end
            
    %主成份分析
    %   coeff: 特征向量
    %   score: 主成份矩阵
    %   latent: 特征值
    %   explained: 解释度
    XT=X1(start-n+1:start,:);
    XSTD=std(XT);
    [COEFF, SCORE, ~, ~, EXPLAINED] = pca(zscore(XT));
    
    if reqvecnum==0
        cont=0;
        num=0;
        while (cont<reqexplained && num<length(EXPLAINED))
            num=num+1;
            cont=cont+EXPLAINED(num);
        end
    else
        num = min(reqvecnum,length(EXPLAINED));
    end
    
    %特征向量
    VEC=COEFF(:,1:num);
    [row3,~]=size(VEC);
    XSTD=XSTD(1:num);  %标准差
    
    %股票指数: 通过主成份构建
    XN1 = SCORE(1:n,1:num);
    [~,col1]=size(XN1);
    
    %债券指数
    XN2 = (X2(start-n+1:start,1));
    [~,col2]=size(XN2);

    %自变量个数
    m=col1+col2;
    
    %构建不等式约束条件：  AX    <     B
    %   0	sum(ai1)    sum(ai2)    1           tposmax
    %   0  -sum(ai1)   -sum(ai2)    0          -eposmin
    %   0   0           0           1           bposmax
    %   0   0           0          -1           0
    %   0  -a11        -a12         0           0
    %  ...                          0           0
    %   0  -an1        -an2         0           0
    
    A=[ sum(VEC),ones(1,col2);
       -sum(VEC),zeros(1,col2);
        zeros(1,col1),ones(1,col2);
        zeros(1,col1),-ones(1,col2);
        -VEC,zeros(size(VEC),col2)
      ];
    A=[zeros(size(A),1),A];
    
    B=[ tposmax;
       -eposmin;
        bposmax;
        0;
        zeros(size(VEC),1);
        ];
    
    %构建回归变量:
    XN=[ones(n,1),XN1,XN2];  %第一列为全1，表示常数项
    YN=(Y(start-n+1:start));

    %带约束的线性回归：最小二乘法
    [BETA,ssr] = lsqlin(XN,YN,A,B);
    
    %计算R^2
    sst=sum((YN-mean(YN)).^2);
    rsq = 1-ssr/sst;
%     rsq = 1-(n/(n-m))*(1-rsq);  %调整的R^2
    
    %行业配置系数
    IPOS = (VEC./repmat(XSTD,row3,1))*BETA(2:col1+1);

    %计算仓位
    epos = sum(IPOS);
    bpos = BETA((1+col1+1):(1+m)); 
end

