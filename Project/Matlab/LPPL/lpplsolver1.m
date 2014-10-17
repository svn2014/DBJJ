%================================
%Sover1: 给定时间区间测算tc的范围
%要点：
%   1）随着t2的移动，tc的预测也会移动，因此t2的变动不应该太大
%   2）非线性拟合时，设置参数边界可能无法获得最佳匹配，建议不设边界而在结果中筛选
%   3) 最终结果不应该以残差大小的决定，而应该考虑tc的分布
%================================
clear;clc;

%读入数据
data = csvread('D:\Projects\Matlab\LPPL\data\index1.csv',1,2);  %交易日
pxcol=1;    %1=上证综指 2=中小板指 3=创业板综
% t1=1525;t2=1854;
t1=1450;t2=1834;
%时间区间:  交易日
%      1 = 2000-01-05
%   1297 = 2005-06-06   上证综指： 998点
%   1997 = 2007-10-16   上证综指：6124点
%       2125 = 2008-10-29   中小版指：2114点
%       2480 = 2010-04-13   中小版指：6177点
%           2620 = 2010-11-11   中小版指：7493点
%           3124 = 2012-12-04   创业板综： 575点
%           3301 = 2013-08-30   
%指数顺序：
%   上证综指/中小板指/创业板综

% data = csvread('D:\Projects\Matlab\LPPL\data\index2.csv',1,2);
% pxcol=1;    %1=上证综指 2=中小板指 3=创业板综
% t1=2100;t2=2800;
%时间区间:  日历日
%      1 = 2000-01-02
%   1983 = 2005-06-06   上证综指： 998点
%   2845 = 2007-10-16   上证综指：6124点
%       3224 = 2008-10-29   中小版指：2114点
%       3755 = 2010-04-13   中小版指：6177点
%           3967 = 2010-11-11   中小版指：7493点
%           4721 = 2012-12-04   创业板综： 575点
%           4990 = 2013-08-30   
%指数顺序：
%   上证综指/中小板指/创业板综
    
%在已知时间区间中各取15个不同的t1和t2拟合各参数
%每个t1,t2间隔5，拟合结果记于R
%随着t2的移动，tc的预测也会移动
tstep1=2;tstep2=1;
tstepcnt1=5;tstepcnt2=5;
% t1list = t1:tstep1:(t1+tstep1*tstepcnt1);
% t2list = (t2-tstep2*tstepcnt2):tstep2:t2;
t1list=t1;
t2list=t2;

%测试：不变动
% t1list=t1;t2list=t2;
R1=zeros(length(t1list)*length(t2list),12);
R2=R1;

rcount=1;
for j=1:length(t1list)
    for k=1:length(t2list)
        P=data(t1list(j):t2list(k),pxcol);
        
        %----------------------
        %拟合LPPL模型
        %----------------------
        LNP=log(P);
        N =length(LNP);
        T=(1:N)';

        %初始点基边界设置
        %   可能的警告：矩阵接近奇异，即det(Matrix)接近0，解方程组得到的结果可能不正确。RCOND是条件数的倒数，RCOND越小，其越接近奇异！
        %   经验：先随意给一个，然后把算出来的值再当做初值继续算，直到两者相差不大为止
        x0=[0.5;8;N+80];
        % 1)0.1<=m<=0.9     
        % 2)6<=w<=13  
        % 3)tc>t
        %设置边界无法获得最佳匹配，建议不设边界在结果中筛选
        lb=[];
        ub=[];
        %非线性拟合并求解各参数
        options = optimset('Algorithm','Levenberg-Marquardt');
        %[t1list(j) t2list(k)]        
        [para,resnorm, ~, exitflag] = lsqcurvefit(@lpplfitfun,x0,[T LNP],LNP,lb,ub,options); 
        [~, P] = lpplfitfun(para, [T LNP]);
        R1(rcount,:) = [resnorm t1list(j) t2list(k) P' fix(t1list(j)+P(7)) exitflag];
        
        %为了避免无可用结果，设置边界拟合参数作为备选
        lb=[0.0001;6;N+1];
        ub=[0.9999;13;N+100]; 
        [para,resnorm, ~, exitflag] = lsqcurvefit(@lpplfitfun,x0,[T LNP],LNP,lb,ub); 
        [~, P] = lpplfitfun(para, [T LNP]);
        R2(rcount,:) = [resnorm t1list(j) t2list(k) P' fix(t1list(j)+P(7)) exitflag];

        rcount=rcount+1;
    end    
end

%筛选排序：
% 1)0.1<=m<=0.9     
% 2)6<=w<=13  
% 3)tc>t
R1=R1(R1(:,8)<1 & R1(:,8)>0,:); %0<m<1，Col=8
R1=R1(R1(:,10)>0,:);            %tc>0，Col=10

%计算用于作图,多画K点
N=length(data(:,pxcol));
lastpx=data(N,pxcol);
K=100;
if N>=(t2 + K)
    PY=data(t1:t2+K,pxcol);
else
    M=t2+K-N;
    PY=[data(t1:N,pxcol);ones(M,1)*lastpx;];
end;

PT=(1:length(PY))';
plot(PT,PY,'k');
hold on;
plot([t2-t1+1,t2-t1+1],[0,PY(t2-t1+1)],'k');


%红线：无边界约束拟合 + 筛选
[N,~]=size(R1);
for i=1:N
    A=R1(i,4);B=R1(i,5);C=R1(i,6);D=R1(i,7); m=R1(i,8);w=R1(i,9);tc=R1(i,10);
    
    %函数中加入abs处理tc之后的点
    NY=exp(A+B.*(abs(tc-PT)).^m+C.*(abs(tc-PT)).^m.*cos(w.*log(abs(tc-PT)))+D.*(abs(tc-PT)).^m.*sin(w.*log(abs(tc-PT))));
    plot(PT,NY,'r');
end;

%蓝线：边界约束拟合
[N,~]=size(R2);
for i=1:N
    A=R2(i,4);B=R2(i,5);C=R2(i,6);D=R2(i,7); m=R2(i,8);w=R2(i,9);tc=R2(i,10);
    
    %函数中加入abs处理tc之后的点
    NY=exp(A+B.*(abs(tc-PT)).^m+C.*(abs(tc-PT)).^m.*cos(w.*log(abs(tc-PT)))+D.*(abs(tc-PT)).^m.*sin(w.*log(abs(tc-PT))));
    plot(PT,NY,'b');
end;
    
    
    
    




    %考虑使用LM算法
    %LOMB频谱分析，[H,q]分析，Ornstein-Uhlenbeck mean-reverting