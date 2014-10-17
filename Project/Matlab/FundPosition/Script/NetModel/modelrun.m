function [EST,IND,RSQ] = modelrun(YN, XN, model, step)
%===============================
%   Y:      ����ֵ�ǵ������У���ʱ����������
%   X:      ָ���ǵ������У���ʱ����������
%   model:  ģ�ʹ���
%   step:   ģ�Ͳ���
%===============================
    %����ʾWarning
    warning('off');

    %����������
    [~,colind] = size(XN);
    [rowf, colf] = size(YN);

    %��������λ�����ͳ��
    EST=zeros(rowf, colf);
    IND=zeros(rowf, colind);
    RSQ=zeros(rowf, colf);
    X0 = [];
    
    n=step;
    for i=n:rowf
        for j=1:colf
            Y=YN(:,j);
            X=XN;
            
            %������
            switch model
                case 1
                    [epos, IPOS, rsqr] = model1(Y, X, n, i, X0);
                case 2
                    [epos, IPOS, rsqr] = model2(Y, X, n, i, X0);
                otherwise
                    throw(MException('Model:InputError','MODEL�Ҳ�����ģ��'));
            end

            EST(i,j)=epos;
            IND(i,:)=IPOS';
            RSQ(i,j)=rsqr;
        end
    end  
end