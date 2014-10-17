function [EST,IND,ERR,RSQ,REAL] = modelrun(model)
    %不显示Warning
    warning('off');

    %设置参数
%     model=3;        %1=多项指数回归   2=复合指数回归    
    switch model
        case 1
            step=30;
            indextype=1;    %1=申万一级行业（28）
        case 2
            step=15;
            indextype=5;    %5=中证100/300/500
        otherwise
            step=28;
            indextype=1;
    end
    indexopt=[];    
    fundtype=0;     %0=可忽略基金类型， 1=股票型    
    usefundgroup=1; %1=全部基金总和
    
    %是否仅在季报公布日测算以统计误差
    checkerronly=0;
    
    %初始化进度条
    hwait = waitbar(0,'0%');
    
    %读入数据，按时间升序排列
    [YN, X1N, X2N, PN, ~] = getdata(usefundgroup,indextype,indexopt);

    %检查输入参数
    [~,colind] = size(X1N);
    [rowf, colf] = size(YN);

    %测算基金仓位和误差统计
    EST=zeros(rowf, colf);
    IND=zeros(rowf, colind);
    RSQ=zeros(rowf, colf);
    ERR=zeros(rowf, colf);
    REAL=PN;
    X0=[];  %迭代初始值
    
    n=step;
    for i=n:rowf
        if checkerronly==1 && sum(PN(i,:)) > 0 || checkerronly~=1
            for j=1:colf
                Y=YN(:,j);         
                %主程序
                [epos, ~, IPOS, rsqr, BETA] = estfundpos(Y, X1N, X2N, n,fundtype, model, i, X0);
                X0=BETA;    %迭代初始值
                
                EST(i,j)=epos;
                IND(i,:)=IPOS';
                RSQ(i,j)=rsqr;
            end

            %误差统计
            if sum(PN(i,:)) > 0
                ERR(i,:) = EST(i,:) - PN(i,:);
            end
        end

        %更新进度条
        waitbar((i-n+1)/(rowf-n+1),hwait,['M',num2str(model),'-I',num2str(indextype),'-S',num2str(n),':  ',num2str(floor((i-n+1)/(rowf-n+1)*100)),'%']);
    end

    %总结误差
    errhist=ERR(sum(ERR,2)~=0,:);
    
    %关闭进度条
    close(hwait);
    
    %作图
    subplot(2,2,1);
    plot(EST(:,1));hold on;plot(REAL(:,1));    
    hold off;
    title('仓位预测');
    subplot(2,2,2);
    plot(RSQ(:,1));
    title('拟合优度');
    subplot(2,2,3);
    plot(ERR(:,1));
    title('预测误差');
    subplot(2,2,4);
    hist(errhist);
    title('误差分布');
    set(gcf,'outerposition',get(0,'screensize'));    
end