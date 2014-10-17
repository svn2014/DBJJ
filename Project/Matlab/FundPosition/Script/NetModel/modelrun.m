function [EST,IND,RSQ] = modelrun(YN, XN, model, step)
%===============================
%   Y:      基金净值涨跌幅序列，按时间升序排列
%   X:      指数涨跌幅序列，按时间升序排列
%   model:  模型代号
%   step:   模型步长
%===============================
    %不显示Warning
    warning('off');

    %检查输入参数
    [~,colind] = size(XN);
    [rowf, colf] = size(YN);

    %测算基金仓位和误差统计
    EST=zeros(rowf, colf);
    IND=zeros(rowf, colind);
    RSQ=zeros(rowf, colf);
    X0 = [];
    
    n=step;
    for i=n:rowf
        for j=1:colf
            Y=YN(:,j);
            X=XN;
            
            %主程序
            switch model
                case 1
                    [epos, IPOS, rsqr] = model1(Y, X, n, i, X0);
                case 2
                    [epos, IPOS, rsqr] = model2(Y, X, n, i, X0);
                otherwise
                    throw(MException('Model:InputError','MODEL找不到该模型'));
            end

            EST(i,j)=epos;
            IND(i,:)=IPOS';
            RSQ(i,j)=rsqr;
        end
    end  
end