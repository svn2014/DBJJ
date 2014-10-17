function [ epos, bpos, IPOS, rsq, BETA] = estfundpos(Y, X1, X2, step, type, model, start, X0)
%============================================
%����Ĺ�Ʊծȯ��λ����ҵ���ò��㹤��
%============================================
%���룺���������������˳������
%   Y:  ����ֵ������������
%   X1: ��ҵָ��������������
%   X2: ծȯָ��������������
%   step:  ʱ�����г���
%   type: �������ͣ�1=��Ʊ�ͣ�2=�����
%   model: ģ������
%   start: ��ǰ����λ��
%�����
%   epos: ��Ʊ��λ
%   bpos: ծȯ��λ
%   IPOS: ��ҵ��������
%   rsq : �ع���Ͷ�
%ģ�����ͣ�
%   1 = ָ���ع鷨
%   2 = ָ�����ɷݻع鷨
%   3 = �ϳ�ָ���ع鷨
%=================================
    %����ʾWarning
    warning('off');   
    
    BETA=[];
    switch model
        case 1
            [ epos, bpos, IPOS, rsq, BETA] = estfundposmodel1(Y, X1, X2, step, type, start, X0);
        case 2
            [ epos, bpos, IPOS, rsq] = estfundposmodel2(Y, X1, [], step, type, start);            
        case 3
            [ epos, bpos, IPOS, rsq] = estfundposmodel3(Y, X1, X2, step, type, start);
        otherwise
            throw(MException('Model:InputError','MODEL�Ҳ�����ģ��'));
    end

end

