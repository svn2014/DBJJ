function [ERRSUM,ERRLIST,RSQLIST] = modeltest(model)
    %不显示Warning
    warning('off');

    %设置参数
    fundtype=0;     %0=可忽略基金类型， 1=股票型
    stepmin=10;
    stepmax=50;
    
    %是否仅在季报公布日测算以统计误差
    checkerronly=1;
    
    %初始化进度条
    hwait = waitbar(0,'0%');    
    ERRSUM=zeros((stepmax-stepmin+1)*6,8);
    ERRLIST=zeros((stepmax-stepmin+1)*6,34);
    RSQLIST=zeros((stepmax-stepmin+1)*6,34);
    
    for indextype=1:15
        %读入数据，按时间升序排列
        [YN, X1N, X2N, PN, ~] = getdata(1,indextype,4);

        %检查输入参数
        [~, coli] = size(X1N);
        [rowf, colf] = size(YN);
        
        %复合指数回归
        if model==2
            step0=stepmin;
        else
            step0=max(stepmin,coli+3);    
        end
        

        %测算基金仓位和误差统计
        estpos=zeros(rowf, colf);
        estrsqr=zeros(rowf, colf);
        esterr=zeros(rowf, colf);
        X0=[];
        
        for n=step0:stepmax
            for i=n:rowf
                if checkerronly==1 && sum(PN(i,:)) > 0
                    for j=1:colf
                        Y=YN(:,j);         
                        %主程序
                        [epos, ~, ~, rsqr, BETA] = estfundpos(Y, X1N, X2N, n,fundtype, model, i, X0);
                        X0=BETA;
                        
                        estpos(i,j)=epos;
                        estrsqr(i,j)=rsqr;
                    end

                    %误差统计
                    if sum(PN(i,:)) > 0
                        esterr(i,:) = estpos(i,:) - PN(i,:);
                    end
                end
                
                %更新进度条
                waitbar((i-n+1)/(rowf-n+1),hwait,['M',num2str(model),'-I',num2str(indextype),'-S',num2str(n),':  ',num2str(floor((i-n+1)/(rowf-n+1)*100)),'%']);
            end

            %总结误差
            errhist=esterr(sum(esterr,2)~=0,:);
            rsqrhist=estrsqr(sum(esterr,2)~=0,:);

            %误差的统计量
            ERRLIST((n-step0+1)+(indextype-1)*(stepmax-stepmin+1),:)=[indextype, n, errhist'];
            RSQLIST((n-step0+1)+(indextype-1)*(stepmax-stepmin+1),:)=[indextype, n, rsqrhist'];
            ERRSUM((n-step0+1)+(indextype-1)*(stepmax-stepmin+1),:) = [indextype, n, mean(errhist), mean(abs(errhist)), std(errhist), max(abs(errhist)), length(errhist(abs(errhist)<0.05))/length(errhist),mean(rsqrhist)];    
        end
    end
    
    %关闭进度条
    close(hwait);
end