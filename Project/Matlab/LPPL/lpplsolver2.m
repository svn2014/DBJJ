%================================
%Sover1: 给定时间区间测算tc的范围
%要点：
%   1）随着t2的移动，tc的预测也会移动，因此t2的变动不应该太大
%   2）非线性拟合时，设置参数边界可能无法获得最佳匹配，建议不设边界而在结果中筛选
%   3) 最终结果不应该以残差大小的决定，而应该考虑tc的分布
%================================
clear;

%读入数据
% data = csvread('D:\Projects\Matlab\LPPL\data\index1.csv',1,2);  %交易日
% pxcol=2;    %1=上证综指 2=中小板指 3=创业板综
% gstart=2325;%设置时间起点
% gend=2800;  %设置时间终点
%时间区间:  交易日
%      1 = 2000-01-05
%   1297 = 2005-06-06   上证综指： 998点
%   1997 = 2007-10-16   上证综指：6124点
%   2125 = 2008-10-29   中小版指：2114点
%   2480 = 2010-04-13   中小版指：6177点
%   2620 = 2010-11-11   中小版指：7493点
%   3124 = 2012-12-04   创业板综： 575点
%   3301 = 2013-08-30   
%指数顺序：
%   上证综指/中小板指/创业板综

data = csvread('D:\Projects\Matlab\LPPL\data\index2.csv',1,2);
pxcol=2;    %1=上证综指 2=中小板指 3=创业板综
gstart=2325;%设置时间起点
gend=2800;  %设置时间终点
%时间区间:  日历日
%      1 = 2000-01-02
%   1983 = 2005-06-06   上证综指： 998点
%   2845 = 2007-10-16   上证综指：6124点
%   3224 = 2008-10-29   中小版指：2114点
%   3755 = 2010-04-13   中小版指：6177点
%   3967 = 2010-11-11   中小版指：7493点
%   4721 = 2012-12-04   创业板综： 575点
%   4990 = 2013-08-30   
%指数顺序：
%   上证综指/中小板指/创业板综


gstep=5;    %设置步长
gminlen=60; %设置最小数据点

%设置测试时间点
% t1list=gstart:gstep:(gstart+gstep*5);
t1list=gstart;

%结果记录
R1=zeros(length(t1list)*(gend-(gstart+60)+1)/gstep,12);
R2=R1;
rcount=1;

%初始化进度条
hwait = waitbar(0,'0%');
    
for i=1:length(t1list)
    t1=t1list(i);
    t2list=(t1+gminlen):gstep:gend;
    for j=1:length(t2list)
        t2=t2list(j);
        P=data(t1:t2,pxcol);
        
        %----------------------
        %拟合LPPL模型
        %----------------------
        LNP=log(P);
        N =length(LNP);
        T=(1:N)';

        para=[0.5;8;N+40];  %初始点随意设置        
        lb=[];              %设置边界无法获得最佳匹配，建议不设边界在结果中筛选
        ub=[];
        options = optimset('Algorithm','Levenberg-Marquardt');
        [para,resnorm] = lsqcurvefit(@lpplfitfun,para,[T LNP],LNP,lb,ub,options); 
        [~, P] = lpplfitfun(para, [T LNP]);
        % 1)0.1<=m<=0.9     
        % 2)6<=w<=13  
        % 3)tc>t
        B=P(2); m = P(5); w=P(6); tc=P(7);
        if m>0 & m<1
            flag=1;
        else
            flag=0;
        end;        
        R1(rcount,:) = [resnorm t1 t2 P' fix(t1+tc) flag];
        
        %为了避免无可用结果，设置边界拟合参数作为备选
        lb=[0.01;6;N+1];
        ub=[0.9;13;N+100]; 
        [para,resnorm] = lsqcurvefit(@lpplfitfun,para,[T LNP],LNP,lb,ub); 
        [~, P] = lpplfitfun(para, [T LNP]);
        R2(rcount,:) = [resnorm t1 t2 P' fix(t1+P(7)) 1];
        
        rcount=rcount+1;
        
        %更新进度条
        len1=length(t1list);
        len2=length(t2list);
        waitbar(((i-1)*len1+j)/(len1*len2),hwait,[num2str(fix(((i-1)*len1+j)/(len1*len2)*100)),'%']);
    end;
end;

%关闭进度条
close(hwait);

%筛选排序：
% 1)0.1<=m<=0.9     
% 2)6<=w<=13  
% 3)tc>t
R1=R1(R1(:,8)<1 & R1(:,8)>0,:); %0<m<1，Col=8

% R1=R1(R1(:,1)>0,:);
% R2=R2(R2(:,1)>0,:);

%计算用于作图,多画K点
N=length(data(:,pxcol));
lastpx=data(N,pxcol);
K=100;
t1=gstart;t2=gend;
if N>=(t2 + K)
    PY=data(t1:t2+K,pxcol);
else
    M=t2+K-N;
    PY=[data(t1:N,pxcol);ones(M,1)*lastpx;];
end;

subplot(2,1,1);
PT=(1:length(PY))';
plot(PT,PY,'k');
hold on;
%红线：无边界约束拟合
TC=fix(R1(:,10));
TC=TC(TC<length(PY));
NY=PY(TC);
plot(TC,NY,'rx');
for i=1:length(R1(:,1))
    t2=R1(i,3)-R1(i,2)+1;
    tc=fix(R1(i,10)) ;
    if tc < length(PY)
        yest=PY(tc);
        X1=[t2,t2,tc];
        Y1=[0,yest,yest];
        plot(X1,Y1,'b');
    end;
end;


subplot(2,1,2);
PT=(1:length(PY))';
plot(PT,PY,'k');
hold on;
%蓝线：边界约束拟合
TC=fix(R2(:,10));
TC=TC(TC<length(PY));
NY=PY(TC);
plot(TC,NY,'bo');
for i=1:length(R2(:,1))
    t2=R2(i,3)-R2(i,2)+1;
    tc=fix(R2(i,10)) ;
    if tc < length(PY)
        yest=PY(tc);
        X1=[t2,t2,tc];
        Y1=[0,yest,yest];
        plot(X1,Y1,'r');
    end;
end;
%LOMB频谱分析，[H,q]分析，Ornstein-Uhlenbeck mean-reverting    